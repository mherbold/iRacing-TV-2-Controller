
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace iRacingTV
{
	public class ViewControl_HeaderData : Control
	{
		public int ScrollIndex { get; set; } = 0;

		private readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
		private readonly Typeface typeface = new( "Courier New" );

		static ViewControl_HeaderData()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof( ViewControl_HeaderData ), new FrameworkPropertyMetadata( typeof( ViewControl_HeaderData ) ) );
		}

		protected override void OnRender( DrawingContext drawingContext )
		{
			base.OnRender( drawingContext );

			var irsdk = App.Instance?.IRSDK;

			if ( ( irsdk != null ) && ( irsdk.IsConnected ) )
			{
				var dictionary = new Dictionary<string, int>()
				{
					{ "Version", irsdk.Data.Version },
					{ "Status", irsdk.Data.Status },
					{ "TickRate", irsdk.Data.TickRate },
					{ "SessionInfoUpdate", irsdk.Data.SessionInfoUpdate },
					{ "SessionInfoLength", irsdk.Data.SessionInfoLength },
					{ "SessionInfoOffset", irsdk.Data.SessionInfoOffset },
					{ "VarCount", irsdk.Data.VarCount },
					{ "VarHeaderOffset", irsdk.Data.VarHeaderOffset },
					{ "BufferCount", irsdk.Data.BufferCount },
					{ "BufferLength", irsdk.Data.BufferLength },
					{ "TickCount", irsdk.Data.TickCount },
					{ "Offset", irsdk.Data.Offset },
					{ "FramesDropped", irsdk.Data.FramesDropped }
				};

				var point = new Point( 10, 0 );
				var lineIndex = 0;

				foreach ( var keyValuePair in dictionary )
				{
					var brush = ( ( lineIndex & 1 ) == 1 ) ? Brushes.AliceBlue : Brushes.White;

					drawingContext.DrawRectangle( brush, null, new Rect( 0, point.Y, ActualWidth, 20 ) );

					var formattedText = new FormattedText( keyValuePair.Key, cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
					{
						LineHeight = 20
					};

					drawingContext.DrawText( formattedText, point );

					point.X += 150;

					formattedText = new FormattedText( keyValuePair.Value.ToString(), cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
					{
						LineHeight = 20
					};

					drawingContext.DrawText( formattedText, point );

					point.X = 10;
					point.Y += 20;

					lineIndex++;
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
	}
}
