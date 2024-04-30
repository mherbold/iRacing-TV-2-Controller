
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace iRacingTV
{
	internal class XmlFile
	{
		public static object Load( string filePath, Type type )
		{
			Debug.WriteLine( $"XmlFile Load({filePath})" );

			var xmlSerializer = new XmlSerializer( type );

			var fileStream = new FileStream( filePath, FileMode.Open );

			var data = xmlSerializer.Deserialize( fileStream ) ?? throw new Exception( $"Failed to deserialize XML file {filePath}." );

			fileStream.Close();

			return data;
		}

		public static void Save( string filePath, object data )
		{
			Debug.WriteLine( $"XmlFile Save({filePath})" );

			var folderPath = Path.GetDirectoryName( filePath ) ?? throw new Exception( $"Failed to get folder path from file path {filePath}." );

			Directory.CreateDirectory( folderPath );

			var xmlSerializer = new XmlSerializer( data.GetType() );

			var streamWriter = new StreamWriter( filePath );

			xmlSerializer.Serialize( streamWriter, data );

			streamWriter.Close();
		}
	}
}
