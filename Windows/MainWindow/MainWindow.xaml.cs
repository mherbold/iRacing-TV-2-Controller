
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace iRacingTV
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			Layers_TreeView_Initialize();
			SessionInfo_ScrollBar_Initialize();
			TelemetryData_ScrollBar_Initialize();

			App.Instance?.Start();
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			App.Instance?.Stop();
		}

		private static T? FindAncestor<T>( DependencyObject current ) where T : DependencyObject
		{
			do
			{
				if ( current is T t )
				{
					return t;
				}

				current = VisualTreeHelper.GetParent( current );
			}
			while ( current != null );

			return null;
		}
	}
}
