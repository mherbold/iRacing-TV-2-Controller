
using System.Windows.Controls.Primitives;

namespace iRacingTV
{
	public partial class MainWindow
	{
		private void SessionInfo_ScrollBar_Initialize()
		{
			SessionInfo_ViewControl.SetScrollBar( SessionInfo_ScrollBar );
		}

		private void SessionInfo_ScrollBar_Scroll( object sender, ScrollEventArgs e )
		{
			SessionInfo_ViewControl.ScrollIndex = (int) e.NewValue;

			SessionInfo_ViewControl.InvalidateVisual();
		}
	}
}
