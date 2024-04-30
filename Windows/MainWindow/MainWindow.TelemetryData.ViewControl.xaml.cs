
using System.Windows.Input;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void TelemetryData_ViewControl_MouseWheel( object sender, MouseWheelEventArgs e )
		{
			TelemetryData_ScrollBar.Value -= e.Delta * 0.125f;

			TelemetryData_ViewControl.ScrollIndex = (int) TelemetryData_ScrollBar.Value;

			TelemetryData_ViewControl.InvalidateVisual();
		}
	}
}
