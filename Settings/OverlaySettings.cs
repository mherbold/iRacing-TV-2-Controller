
using System.Collections.ObjectModel;
using System.ComponentModel;

using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

using static iRacingTV.Unity;

namespace iRacingTV
{
	public class OverlaySettings : Settings
	{
		// consts

		public const string DefaultOverlaySettingsFolderName = "Overlay Settings\\";
		public const string DefaultOverlaySettingsFileName = "My Overlay Settings.xml";

		public static readonly string defaultOverlaySettingsFolderPath = App.documentsFolderPath + DefaultOverlaySettingsFolderName;

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

				var filePath = $"{defaultOverlaySettingsFolderPath}{_fileName}.xml";

				App.Instance?.SetOverlaySettings( (OverlaySettings) Load( filePath, typeof( OverlaySettings ) ) );
			}
		}

		public static void Select( string filePath )
		{
			FileName = App.Instance?.OverlaySettingsFileList.Add( filePath ) ?? string.Empty;
		}

		// Position

		[Category( "Window" ), ExpandableObject]
		public Vector2Int Position { get; set; } = Vector2Int.zero;

		// Size

		[Category( "Window" ), ExpandableObject]
		public Vector2Int Size { get; set; } = new() { X = 1920, Y = 1080 };

		// Fonts

		[Category( "Fonts" ), ExpandableObject]
		[DisplayName( "Fonts" )]
		public ObservableCollection<Font> FontDictionary { get; set; } = new();
	}
}
