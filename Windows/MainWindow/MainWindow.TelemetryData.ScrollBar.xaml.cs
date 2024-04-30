
using System.Windows.Controls.Primitives;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void TelemetryData_ScrollBar_Initialize()
		{
			TelemetryData_ViewControl.SetScrollBar( TelemetryData_ScrollBar );
		}

		private void TelemetryData_ScrollBar_Scroll( object sender, ScrollEventArgs e )
		{
			TelemetryData_ViewControl.ScrollIndex = (int) e.NewValue;

			TelemetryData_ViewControl.InvalidateVisual();
		}
	}
}
