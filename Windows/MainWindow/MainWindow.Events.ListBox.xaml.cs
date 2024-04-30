using HerboldRacing;

using System.Collections.Generic;
using System.Windows.Controls;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void Events_ListBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			if ( sender is ListBox listBox )
			{
				if ( listBox.SelectedItem is KeyValuePair<string, EventSystem.EventTrack> keyValuePair )
				{
					Events_DataGrid.ItemsSource = keyValuePair.Value.Events;
				}
			}
		}
	}
}
