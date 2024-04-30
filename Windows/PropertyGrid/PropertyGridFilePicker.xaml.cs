
using Microsoft.Win32;

using System.Windows.Data;
using System.Windows;

using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace iRacingTV
{
	public partial class PropertyGridFilePicker : ITypeEditor
	{
		public PropertyGridFilePicker()
		{
			InitializeComponent();
		}

		public string Value
		{
			get { return (string) GetValue( ValueProperty ); }
			set { SetValue( ValueProperty, value ); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof( string ), typeof( PropertyGridFilePicker ), new PropertyMetadata( null ) );

		public FrameworkElement ResolveEditor( PropertyItem propertyItem )
		{
			var binding = new Binding( "Value" )
			{
				Source = propertyItem,
				Mode = propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
			};

			BindingOperations.SetBinding( this, ValueProperty, binding );

			return this;
		}

		private void PickFileButton_Click( object sender, RoutedEventArgs e )
		{
			var openFileDialog = new OpenFileDialog();

			if ( ( openFileDialog.ShowDialog() == true ) && openFileDialog.CheckFileExists )
			{
				Value = openFileDialog.FileName;
			}
		}
	}
}
