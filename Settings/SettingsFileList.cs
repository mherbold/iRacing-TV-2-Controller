
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace iRacingTV
{
	public class SettingsFileList
	{
		public ObservableCollection<string> FileList { get; private set; } = new();

		public SettingsFileList( string folderPath )
		{
			if ( !Directory.Exists( folderPath ) )
			{
				Directory.CreateDirectory( folderPath );
			}

			var filePaths = Directory.EnumerateFiles( folderPath );

			foreach ( var filePath in filePaths )
			{
				if ( filePath.EndsWith( ".xml" ) )
				{
					Add( filePath );
				}
			}
		}

		public string Add( string filePath )
		{
			var fileName = Path.GetFileName( filePath )[ ..^4 ];

			if ( !FileList.Contains( fileName ) )
			{
				FileList.Add( fileName );
			}

			return fileName;
		}
	}
}
