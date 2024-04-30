
using System.Windows;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void Layers_TreeView_ContextMenu_NewLayer_Container_Click( object sender, RoutedEventArgs e )
		{
			var layer = new Layer()
			{
				Name = "New Container"
			};

			AddLayer( layer );
		}

		private void Layers_TreeView_ContextMenu_NewLayer_Image_File_Click( object sender, RoutedEventArgs e )
		{
			var layer = new FileImageLayer()
			{
				Name = "New File Image Layer"
			};

			AddLayer( layer );
		}

		private void Layers_TreeView_ContextMenu_NewLayer_Image_Streamed_Weekend_Click( object sender, RoutedEventArgs e )
		{
			var layer = new WeekendStreamedImageLayer()
			{
				Name = "New Weekend Streamed Image Layer"
			};

			AddLayer( layer );
		}

		private void Layers_TreeView_ContextMenu_NewLayer_Image_Streamed_Driver_Click( object sender, RoutedEventArgs e )
		{
			var layer = new DriverStreamedImageLayer()
			{
				Name = "New Driver Streamed Image Layer"
			};

			AddLayer( layer );
		}

		private void AddLayer<T>(T layer) where T : Layer
		{
			var selectedLayer = Overlays_TreeView.SelectedItem as Layer;

			if ( selectedLayer != null )
			{
				selectedLayer.AddChild( layer );

				if ( overlays_treeView_contextMenuTreeViewItem != null )
				{
					overlays_treeView_contextMenuTreeViewItem.IsExpanded = true;
				}
			}
		}
	}
}
