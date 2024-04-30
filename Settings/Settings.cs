
using System;
using System.IO;

namespace iRacingTV
{
	public class Settings
	{
		private string filePath = string.Empty;
		private bool saveQueued = false;
		private int secondsToSave = 0;

		public void Initialize()
		{

		}

		public void BeginSave()
		{
			saveQueued = true;
			secondsToSave = 5;
		}

		public void OnTick( bool isStopping )
		{
			if ( saveQueued )
			{
				secondsToSave--;

				if ( ( secondsToSave <= 0 ) || isStopping )
				{
					saveQueued = false;
					secondsToSave = 0;

					XmlFile.Save( filePath, this );
				}
			}
		}

		public static object Load( string filePath, Type type )
		{
			Settings settings;

			if ( File.Exists( filePath ) )
			{
				settings = (Settings) XmlFile.Load( filePath, type );
			}
			else
			{
				settings = (Settings) ( Activator.CreateInstance( type ) ?? throw new Exception( $"Failed to create an instance of type {type}." ) );
			}

			settings.filePath = filePath;

			return settings;
		}
	}
}
