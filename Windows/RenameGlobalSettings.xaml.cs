
using System.ComponentModel;
using System.Windows;

namespace iRacingTV
{
	public partial class RenameGlobalSettings : Window
	{
		private static RenameGlobalSettings? Instance = null;

		public string FileName { get; set; }

		public static void Open()
		{
			if ( Instance == null )
			{
				var renameGlobalSettings = new RenameGlobalSettings( GeneralSettings.FileName );

				renameGlobalSettings.Show();
			}
		}

		private RenameGlobalSettings( string fileName )
		{
			InitializeComponent();

			Instance = this;
			DataContext = this;
			FileName = fileName;
			Owner = App.Instance?.MainWindow ?? null;
		}

		private void Window_Closing( object sender, CancelEventArgs e )
		{
			Instance = null;
		}

		private void Rename_Click( object sender, RoutedEventArgs e )
		{
			Close();
		}

		private void Cancel_Click( object sender, RoutedEventArgs e )
		{
			Close();
		}
	}
}
