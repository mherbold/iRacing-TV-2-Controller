
namespace iRacingTV
{
	public class LastUsedSettings : Settings
	{
		public const string LastUsedSettingsFileName = "LastUsed.xml";

		public static readonly string lastUsedSettingsFilePath = App.documentsFolderPath + LastUsedSettingsFileName;

		public string lastUsedGeneralSettingsPath = GeneralSettings.defaultGeneralSettingsFolderPath + GeneralSettings.DefaultGeneralSettingsFileName;
		public string lastUsedOverlaySettingsPath = OverlaySettings.defaultOverlaySettingsFolderPath + OverlaySettings.DefaultOverlaySettingsFileName;

		public static LastUsedSettings Load()
		{
			return (LastUsedSettings) Load( lastUsedSettingsFilePath, typeof( LastUsedSettings ) );
		}
	}
}
