#pragma warning disable 8602, 8604

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace iRacingTV
{
	public partial class MainWindow
	{
		TreeViewItem? overlays_treeView_treeViewItemBeingDragged = null;
		TreeViewItem? overlays_treeView_contextMenuTreeViewItem = null;
		Layer? overlays_treeView_layerBeingDragged = null;
		Point overlays_treeView_layerDragPoint = new( 0, 0 );
		TreeViewItemAdorner? overlays_treeView_currentTreeViewItemAdorner = null;

		public void Layers_TreeView_Initialize()
		{
			var rootLayer = new Layer() { Name = "Root" };

			Overlays_TreeView.Items.Add( rootLayer );
		}

		private void Layers_TreeView_PreviewMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
		{
			overlays_treeView_treeViewItemBeingDragged = null;
			overlays_treeView_layerBeingDragged = null;

			if ( sender as TreeView == null )
			{
				return;
			}

			overlays_treeView_treeViewItemBeingDragged = FindAncestor<TreeViewItem>( (DependencyObject) e.OriginalSource );

			if ( overlays_treeView_treeViewItemBeingDragged == null )
			{
				return;
			}

			overlays_treeView_layerBeingDragged = overlays_treeView_treeViewItemBeingDragged.Header as Layer;

			if ( overlays_treeView_layerBeingDragged == null )
			{
				return;
			}

			if ( overlays_treeView_layerBeingDragged.IsRoot )
			{
				overlays_treeView_layerBeingDragged = null;

				return;
			}

			overlays_treeView_layerDragPoint = e.GetPosition( null );
		}

		private void Layers_TreeView_PreviewMouseRightButtonDown( object sender, MouseButtonEventArgs e )
		{
			overlays_treeView_contextMenuTreeViewItem = FindAncestor<TreeViewItem>( (DependencyObject) e.OriginalSource );

			if ( overlays_treeView_contextMenuTreeViewItem != null )
			{
				overlays_treeView_contextMenuTreeViewItem.Focus();

				e.Handled = true;
			}
		}

		private void Layers_TreeView_MouseMove( object sender, MouseEventArgs e )
		{
			if ( ( e.LeftButton == MouseButtonState.Pressed ) && ( overlays_treeView_layerBeingDragged != null ) )
			{
				var currentPoint = e.GetPosition( null );

				var deltaPoint = overlays_treeView_layerDragPoint - currentPoint;

				if ( ( Math.Abs( deltaPoint.X ) > SystemParameters.MinimumHorizontalDragDistance ) || ( Math.Abs( deltaPoint.Y ) > SystemParameters.MinimumVerticalDragDistance ) )
				{
					DragDrop.DoDragDrop( overlays_treeView_treeViewItemBeingDragged, overlays_treeView_layerBeingDragged, DragDropEffects.Copy | DragDropEffects.Move );
				}
			}
		}

		private void Layers_TreeView_DragEnter( object sender, DragEventArgs e )
		{
			if ( GetTreeViewItemAndLayers( e, out var treeViewItem, out var layer, out var targetLayer, out var adornerLayer ) )
			{
				overlays_treeView_currentTreeViewItemAdorner = new TreeViewItemAdorner( treeViewItem );

				adornerLayer.Add( overlays_treeView_currentTreeViewItemAdorner );

				UpdateEffectsAndAdorner( e, treeViewItem, layer, targetLayer );
			}
		}

		private void Layers_TreeView_DragLeave( object sender, DragEventArgs e )
		{
			if ( GetTreeViewItemAndLayers( e, out _, out _, out _, out var adornerLayer ) )
			{
				adornerLayer.Remove( overlays_treeView_currentTreeViewItemAdorner );

				overlays_treeView_currentTreeViewItemAdorner = null;
			}
		}

		private void Layers_TreeView_DragOver( object sender, DragEventArgs e )
		{
			if ( GetTreeViewItemAndLayers( e, out var treeViewItem, out var layer, out var targetLayer, out _ ) )
			{
				UpdateEffectsAndAdorner( e, treeViewItem, layer, targetLayer );
			}
			else
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
			}
		}

		private void Layers_TreeView_Drop( object sender, DragEventArgs e )
		{
			if ( GetTreeViewItemAndLayers( e, out var treeViewItem, out var layer, out var targetLayer, out var adornerLayer ) )
			{
				var isCloning = Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl );

				if ( CanDrop( e, treeViewItem, layer, targetLayer, isCloning, out var dropLayer, out bool isDroppingAbove ) )
				{
					if ( isCloning )
					{
						layer = (Layer) layer.Clone();
					}

					if ( isDroppingAbove )
					{
						dropLayer.InsertChild( layer, targetLayer );
					}
					else
					{
						dropLayer.AddChild( layer );
					}
				}

				adornerLayer.Remove( overlays_treeView_currentTreeViewItemAdorner );

				overlays_treeView_currentTreeViewItemAdorner = null;
			}
		}

		private void Layers_TreeView_KeyDown( object sender, KeyEventArgs e )
		{
			if ( sender is not TreeView treeView )
			{
				return;
			}

			if ( e.Key == Key.Delete )
			{
				var layer = treeView.SelectedItem as Layer;

				layer?.Remove();
			}
		}

		private void Layers_TreeView_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
		{
			Overlays_PropertyGrid.SelectedObject = Overlays_TreeView.SelectedItem;
		}

		private void UpdateEffectsAndAdorner( DragEventArgs e, TreeViewItem treeViewItem, Layer layer, Layer targetLayer )
		{
			var isCloning = Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl );

			if ( CanDrop( e, treeViewItem, layer, targetLayer, isCloning, out _, out var isDroppingAbove ) )
			{
				e.Effects = ( isCloning ) ? DragDropEffects.Copy : DragDropEffects.Move;

				overlays_treeView_currentTreeViewItemAdorner.Visible = isDroppingAbove;
			}
			else
			{
				e.Effects = DragDropEffects.None;

				overlays_treeView_currentTreeViewItemAdorner.Visible = false;
			}

			e.Handled = true;
		}

		static private bool GetTreeViewItemAndLayers( DragEventArgs e, out TreeViewItem? treeViewItem, out Layer? layer, out Layer? targetLayer, out AdornerLayer? adornerLayer )
		{
			treeViewItem = null;
			targetLayer = null;
			adornerLayer = null;

			var formats = e.Data.GetFormats();

			layer = e.Data.GetData( formats[ 0 ] ) as Layer;

			if ( layer != null )
			{
				treeViewItem = FindAncestor<TreeViewItem>( (DependencyObject) e.OriginalSource );

				if ( treeViewItem != null )
				{
					targetLayer = treeViewItem.Header as Layer;

					if ( targetLayer != null )
					{
						adornerLayer = AdornerLayer.GetAdornerLayer( treeViewItem );

						if ( adornerLayer != null )
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		static private bool CanDrop( DragEventArgs e, TreeViewItem treeViewItem, Layer layer, Layer targetLayer, bool isCloning, out Layer? dropLayer, out bool isDroppingAbove )
		{
			var currentPoint = e.GetPosition( treeViewItem );

			isDroppingAbove = currentPoint.Y < 6;

			dropLayer = isDroppingAbove ? targetLayer.Parent : targetLayer;

			return ( isCloning || !ReferenceEquals( targetLayer, layer ) && ( dropLayer != null ) && layer.CanBeChildOf( dropLayer ) );
		}

		public class TreeViewItemAdorner : Adorner
		{
			public bool Visible
			{
				set
				{
					var oldValue = visible;

					visible = value;

					if ( visible != oldValue )
					{
						InvalidateVisual();
					}
				}
			}

			private readonly Pen pen = new()
			{
				Brush = Brushes.CornflowerBlue,
				Thickness = 1,
				DashStyle = new()
				{
					Dashes = { 8, 4 }
				}
			};

			private bool visible;

			public TreeViewItemAdorner( UIElement adornedElement ) : base( adornedElement )
			{
				IsHitTestVisible = false;

				visible = false;
			}

			protected override void OnRender( DrawingContext drawingContext )
			{
				if ( visible )
				{
					var adornedElementRect = new Rect( AdornedElement.DesiredSize );

					var topLeft = adornedElementRect.TopLeft;
					var topRight = adornedElementRect.TopRight;

					topLeft.Y += 1;
					topRight.Y += 1;

					drawingContext.DrawLine( pen, topLeft, topRight );
				}
			}
		}
	}
}
