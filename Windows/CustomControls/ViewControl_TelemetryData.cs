
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using HerboldRacing;

namespace iRacingTV
{
	public class ViewControl_TelemetryData : Control
	{
		public int ScrollIndex { get; set; } = 0;

		private ScrollBar? scrollBar = null;

		private readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
		private readonly Typeface typeface = new( "Courier New" );

		static ViewControl_TelemetryData()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof( ViewControl_TelemetryData ), new FrameworkPropertyMetadata( typeof( ViewControl_TelemetryData ) ) );
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

				var point = new Point( 10, 0 );
				var lineIndex = 0;
				var stopDrawing = false;

				foreach ( var keyValuePair in irsdk.Data.TelemetryDataProperties )
				{
					for ( var valueIndex = 0; valueIndex < keyValuePair.Value.Count; valueIndex++ )
					{
						if ( ( lineIndex >= ScrollIndex ) && !stopDrawing )
						{
							var brush = ( ( lineIndex & 1 ) == 1 ) ? Brushes.AliceBlue : Brushes.White;

							drawingContext.DrawRectangle( brush, null, new Rect( 0, point.Y, ActualWidth, 20 ) );

							var offset = keyValuePair.Value.Offset + valueIndex * IRacingSdkConst.VarTypeBytes[ (int) keyValuePair.Value.VarType ];

							var formattedText = new FormattedText( offset.ToString(), cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
							{
								LineHeight = 20
							};

							drawingContext.DrawText( formattedText, point );

							point.X += 40;

							formattedText = new FormattedText( keyValuePair.Value.Name, cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
							{
								LineHeight = 20
							};

							drawingContext.DrawText( formattedText, point );

							point.X += 230;

							if ( keyValuePair.Value.Count > 1 )
							{
								formattedText = new FormattedText( valueIndex.ToString(), cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
								{
									LineHeight = 20
								};

								drawingContext.DrawText( formattedText, point );
							}

							point.X += 30;

							var valueAsString = string.Empty;
							var bitsAsString = string.Empty;

							brush = Brushes.Black;

							switch ( keyValuePair.Value.Unit )
							{
								case "irsdk_TrkLoc":
									valueAsString = GetString<IRacingSdkEnum.TrkLoc>( irsdk, keyValuePair.Value, valueIndex );
									break;

								case "irsdk_TrkSurf":
									valueAsString = GetString<IRacingSdkEnum.TrkSurf>( irsdk, keyValuePair.Value, valueIndex );
									break;

								case "irsdk_SessionState":
									valueAsString = GetString<IRacingSdkEnum.SessionState>( irsdk, keyValuePair.Value, valueIndex );
									break;

								case "irsdk_CarLeftRight":
									valueAsString = GetString<IRacingSdkEnum.CarLeftRight>( irsdk, keyValuePair.Value, valueIndex );
									break;

								case "irsdk_PitSvStatus":
									valueAsString = GetString<IRacingSdkEnum.PitSvStatus>( irsdk, keyValuePair.Value, valueIndex );
									break;

								case "irsdk_PaceMode":
									valueAsString = GetString<IRacingSdkEnum.PaceMode>( irsdk, keyValuePair.Value, valueIndex );
									break;

								default:

									switch ( keyValuePair.Value.VarType )
									{
										case IRacingSdkEnum.VarType.Char:
											valueAsString = $"         {irsdk.Data.GetChar( keyValuePair.Value, valueIndex )}";
											break;

										case IRacingSdkEnum.VarType.Bool:
											var valueAsBool = irsdk.Data.GetBool( keyValuePair.Value, valueIndex );
											valueAsString = valueAsBool ? "         T" : "         F";
											brush = valueAsBool ? Brushes.Green : Brushes.Red;
											break;

										case IRacingSdkEnum.VarType.Int:
											valueAsString = $"{irsdk.Data.GetInt( keyValuePair.Value, valueIndex ),10:N0}";
											break;

										case IRacingSdkEnum.VarType.BitField:
											valueAsString = $"0x{irsdk.Data.GetBitField( keyValuePair.Value, valueIndex ):X8}";

											switch ( keyValuePair.Value.Unit )
											{
												case "irsdk_EngineWarnings":
													bitsAsString = GetString<IRacingSdkEnum.EngineWarnings>( irsdk, keyValuePair.Value, valueIndex );
													break;

												case "irsdk_Flags":
													bitsAsString = GetString<IRacingSdkEnum.Flags>( irsdk, keyValuePair.Value, valueIndex );
													break;

												case "irsdk_CameraState":
													bitsAsString = GetString<IRacingSdkEnum.CameraState>( irsdk, keyValuePair.Value, valueIndex );
													break;

												case "irsdk_PitSvFlags":
													bitsAsString = GetString<IRacingSdkEnum.PitSvFlags>( irsdk, keyValuePair.Value, valueIndex );
													break;

												case "irsdk_PaceFlags":
													bitsAsString = GetString<IRacingSdkEnum.PaceFlags>( irsdk, keyValuePair.Value, valueIndex );
													break;
											}

											break;

										case IRacingSdkEnum.VarType.Float:
											valueAsString = $"{irsdk.Data.GetFloat( keyValuePair.Value, valueIndex ),15:N4}";
											break;

										case IRacingSdkEnum.VarType.Double:
											valueAsString = $"{irsdk.Data.GetDouble( keyValuePair.Value, valueIndex ),15:N4}";
											break;
									}

									break;
							}

							formattedText = new FormattedText( valueAsString, cultureInfo, FlowDirection.LeftToRight, typeface, 12, brush, 1.25f )
							{
								LineHeight = 20
							};

							drawingContext.DrawText( formattedText, point );

							point.X += 150;

							formattedText = new FormattedText( keyValuePair.Value.Unit, cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
							{
								LineHeight = 20
							};

							drawingContext.DrawText( formattedText, point );

							point.X += 160;

							var desc = keyValuePair.Value.Desc;
							var originalDescLength = desc.Length;

							if ( bitsAsString != string.Empty )
							{
								desc += $" ({bitsAsString})";
							}

							formattedText = new FormattedText( desc, cultureInfo, FlowDirection.LeftToRight, typeface, 12, Brushes.Black, 1.25f )
							{
								LineHeight = 20
							};

							if ( bitsAsString != string.Empty )
							{
								formattedText.SetForegroundBrush( Brushes.OrangeRed, originalDescLength, desc.Length - originalDescLength );
							}

							drawingContext.DrawText( formattedText, point );

							point.X = 10;
							point.Y += 20;

							if ( ( point.Y + 20 ) > ActualHeight )
							{
								stopDrawing = true;
							}
						}

						lineIndex++;
					}
				}

				if ( scrollBar != null )
				{
					scrollBar.Maximum = lineIndex - 1;
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

		private static string GetString<T>( IRSDKSharper irsdk, IRacingSdkDatum var, int index ) where T : Enum
		{
			if ( var.VarType == IRacingSdkEnum.VarType.Int )
			{
				var enumValue = (T) (object) irsdk.Data.GetInt( var, index );

				return enumValue.ToString();
			}
			else
			{
				var bits = irsdk.Data.GetBitField( var, index );

				var bitsString = string.Empty;

				foreach ( uint bitMask in Enum.GetValues( typeof( T ) ) )
				{
					if ( ( bits & bitMask ) != 0 )
					{
						if ( bitsString != string.Empty )
						{
							bitsString += " | ";
						}

						bitsString += Enum.GetName( typeof( T ), bitMask );
					}
				}

				return bitsString;
			}
		}
	}
}
