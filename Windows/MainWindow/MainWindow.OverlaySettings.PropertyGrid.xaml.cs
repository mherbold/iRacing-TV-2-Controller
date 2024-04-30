﻿
using System.Windows;

using Xceed.Wpf.Toolkit.PropertyGrid;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void OverlaySettings_PropertyGrid_PropertyValueChanged( object sender, PropertyValueChangedEventArgs e )
		{
			App.Instance?.OverlaySettings.BeginSave();
		}
	}
}
