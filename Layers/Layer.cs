
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

using static iRacingTV.Unity;

namespace iRacingTV
{
	internal class Layer : ICloneable
	{
		public string Name { get; set; } = "Not set";
		[ExpandableObject]
		public Vector2Int Position { get; set; } = Vector2Int.zero;
		public bool Enabled { get; set; } = true;

		[Browsable( false )]
		public bool IsRoot { get { return Parent == null; } }
		[Browsable( false )]
		public ObservableCollection<Layer> Children { get; }
		[Browsable( false )]
		public Layer? Parent { get; private set; } = null;

		public Layer()
		{
			Children = new ObservableCollection<Layer>();
		}

		public void AddChild( Layer layer )
		{
			if ( layer.Parent != null )
			{
				layer.Parent.Children.Remove( layer );
			}

			layer.Parent = this;

			Children.Add( layer );
		}

		public void InsertChild( Layer layer, Layer beforeLayer )
		{
			if ( layer.Parent != null )
			{
				layer.Parent.Children.Remove( layer );
			}

			layer.Parent = this;

			var index = Children.IndexOf( beforeLayer );

			Children.Insert( index, layer );
		}

		public bool CanBeChildOf( Layer? layer )
		{
			if ( ReferenceEquals( layer, this ) )
			{
				return false;
			}

			while ( layer != null )
			{
				if ( ReferenceEquals( layer.Parent, this ) )
				{
					return false;
				}

				layer = layer.Parent;
			}

			return true;
		}

		public object Clone()
		{
			var layer = new Layer
			{
				Name = Name
			};

			foreach ( var child in Children )
			{
				var clone = (Layer) child.Clone();

				layer.AddChild( clone );
			}

			return layer;
		}

		public void Remove()
		{
			Parent?.Children.Remove( this );
		}
	}
}
