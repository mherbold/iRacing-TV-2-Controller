
using System.Windows.Input;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void SessionInfo_ViewControl_MouseWheel( object sender, MouseWheelEventArgs e )
		{
			SessionInfo_ScrollBar.Value -= e.Delta * 0.125f;

			SessionInfo_ViewControl.ScrollIndex = (int) SessionInfo_ScrollBar.Value;

			SessionInfo_ViewControl.InvalidateVisual();
		}
	}
}
