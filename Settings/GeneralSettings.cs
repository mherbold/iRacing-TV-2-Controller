
using System.ComponentModel;
using System.Windows;

using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace iRacingTV
{
	public class GeneralSettings : Settings
	{
		// consts

		public const string DefaultGeneralSettingsFolderName = "General Settings\\";
		public const string DefaultGeneralSettingsFileName = "My General Settings.xml";

		public static readonly string defaultGeneralSettingsFolderPath = App.documentsFolderPath + DefaultGeneralSettingsFolderName;

		public const string DefaultTelemetryFolderName = "Telemetry\\";

		public static readonly string defaultTelemetryFolderPath = App.documentsFolderPath + DefaultTelemetryFolderName;

		// FileName (static)

		private static string _fileName = string.Empty;

		public static string FileName
		{
			get
			{
				return _fileName;
			}

			set
			{
				_fileName = value;

				var filePath = $"{defaultGeneralSettingsFolderPath}{_fileName}.xml";

				App.Instance?.SetGeneralSettings( (GeneralSettings) Load( filePath, typeof( GeneralSettings ) ) );
			}
		}

		public static void Select( string filePath )
		{
			FileName = App.Instance?.GeneralSettingsFileList.Add( filePath ) ?? string.Empty;
		}

		// TelemetryFolder

		private string _telemetryFolder = defaultTelemetryFolderPath;

		[Category( "Telemetry" ), ExpandableObject]
		[DisplayName( "Telemetry folder" )]
		[Editor( typeof( PropertyGridFolderPicker ), typeof( PropertyGridFolderPicker ) )]
		public string TelemetryFolder
		{
			get
			{
				return _telemetryFolder;
			}

			set
			{
				_telemetryFolder = value;

				var app = (App) Application.Current;

				app.IRSDK.EnableEventSystem( _telemetryFolder );
			}
		}

		// AlwaysOnTop

		[Category( "Controller window" )]
		[DisplayName( "Always on top" )]
		public bool AlwaysOnTop { get; set; } = false;
	}
}
