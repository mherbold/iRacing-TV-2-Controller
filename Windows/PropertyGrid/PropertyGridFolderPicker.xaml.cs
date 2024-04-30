
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows;

using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace iRacingTV
{
	public partial class PropertyGridFolderPicker : ITypeEditor
	{
		public PropertyGridFolderPicker()
		{
			InitializeComponent();
		}

		public string Value
		{
			get { return (string) GetValue( ValueProperty ); }
			set { SetValue( ValueProperty, value ); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof( string ), typeof( PropertyGridFolderPicker ), new PropertyMetadata( null ) );

		public FrameworkElement ResolveEditor( PropertyItem propertyItem )
		{
			var binding = new System.Windows.Data.Binding( "Value" )
			{
				Source = propertyItem,
				Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
			};

			BindingOperations.SetBinding( this, ValueProperty, binding );

			return this;
		}

		private void PickFolderButton_Click( object sender, RoutedEventArgs e )
		{
			using ( var dialog = new FolderBrowserDialog() )
			{
				dialog.ShowNewFolderButton = true;
				dialog.AutoUpgradeEnabled = true;
				dialog.ShowPinnedPlaces = true;
				
				DialogResult result = dialog.ShowDialog();

				if ( result == DialogResult.OK )
				{
					Value = dialog.SelectedPath;
				}
			}
		}
	}
}
