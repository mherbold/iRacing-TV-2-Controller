
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using HerboldRacing;

namespace iRacingTV
{
	public class ViewControl_SessionInfo : Control
	{
		public int ScrollIndex { get; set; } = 0;

		private ScrollBar? scrollBar = null;

		private readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
		private readonly Typeface typeface = new( "Courier New" );

		static ViewControl_SessionInfo()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof( ViewControl_SessionInfo ), new FrameworkPropertyMetadata( typeof( ViewControl_SessionInfo ) ) );
		}

		public void SetScrollBar( ScrollBar scrollBar )
		{
			this.scrollBar = scrollBar;
		}

		protected override void OnRender( DrawingContext drawingContext )
		{
			base.OnRender( drawingContext );

			var irsdk = App.Instance?.IRSDK;

			if ( ( irsdk != null ) && ( irsdk.IsConnected ) )
			{
				drawingContext.DrawRectangle( Brushes.Beige, null, new Rect( 0, 0, ActualWidth, ActualHeight ) );

				if ( irsdk.Data.SessionInfo is IRacingSdkSessionInfo sessionInfo )
				{
					var point = new Point( 10, 0 );
					var lineIndex = 0;
					var stopDrawing = false;

					foreach ( var propertyInfo in sessionInfo.GetType().GetProperties() )
					{
						DrawSessionInfo( drawingContext, propertyInfo.Name, propertyInfo.GetValue( sessionInfo ), 0, ref point, ref lineIndex, ref stopDrawing );
					}

					if ( scrollBar != null )
					{
						scrollBar.Maximum = lineIndex - 1;
					}
				}
			}
			else
			{
				drawingContext.DrawRectangle( Brushes.DarkRed, null, new Rect( 0, 0, ActualWidth, ActualHeight ) );

				var formattedText = new FormattedText( "The iRacing Simulator is not running.", cultureInfo, FlowDirection.LeftToRight, typeface, 24, Brushes.White, 1.25f )
				{
					TextAlignment = TextAlignment.Center
				};

				var point = new Point( ActualWidth / 2, ActualHeight / 2 - formattedText.Height / 2 );

				drawingContext.DrawText( formattedText, point );
			}
		}

		private void DrawSessionInfo( DrawingContext drawingContext, string propertyName, object? valueAsObject, int indent, ref Point point, ref int lineIndex, ref bool stopDrawing )
		{
			var isSimpleValue = ( ( valueAsObject is null ) || ( valueAsObject is string ) || ( valueAsObject is int ) || ( valueAsObject is float ) || ( valueAsObject is double ) );

			if ( ( lineIndex >= ScrollIndex ) && !stopDrawing )
			{
				var brush = ( ( lineIndex & 1 ) == 1 ) ? Brushes.AliceBlue : Brushes.White;

				drawingContext.DrawRectangle( brush, null, new Rect( 0, point.Y, ActualWidth, 20 ) );

				point.X = 10 + indent * 50;

				var formattedText = new FormattedText( propertyName, cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
				{
					LineHeight = 20
				};

				drawingContext.DrawText( formattedText, point );

				if ( valueAsObject is null )
				{
					point.X = 260 + indent * 50;

					formattedText = new FormattedText( "(null)", cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
					{
						LineHeight = 20
					};

					drawingContext.DrawText( formattedText, point );
				}
				else if ( isSimpleValue )
				{
					point.X = 260 + indent * 50;

					formattedText = new FormattedText( valueAsObject.ToString(), cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
					{
						LineHeight = 20
					};

					drawingContext.DrawText( formattedText, point );
				}

				point.Y += 20;

				if ( ( point.Y + 20 ) > ActualHeight )
				{
					stopDrawing = true;
				}
			}

			lineIndex++;

			if ( !isSimpleValue )
			{
				if ( valueAsObject is IList list )
				{
					var index = 0;

					foreach ( var item in list )
					{
						DrawSessionInfo( drawingContext, index.ToString(), item, indent + 1, ref point, ref lineIndex, ref stopDrawing );

						index++;
					}
				}
				else
				{
#pragma warning disable CS8602
					foreach ( var propertyInfo in valueAsObject.GetType().GetProperties() )
					{
						DrawSessionInfo( drawingContext, propertyInfo.Name, propertyInfo.GetValue( valueAsObject ), indent + 1, ref point, ref lineIndex, ref stopDrawing );
					}
#pragma warning restore CS8602
				}
			}
		}
	}
}
