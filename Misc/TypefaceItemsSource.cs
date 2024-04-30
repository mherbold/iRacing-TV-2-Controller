
using System;
using System.IO;
using System.Linq;
using System.Windows.Media;

using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace iRacingTV
{
	internal class TypefaceItemsSource : IItemsSource
	{
		public static readonly string globalFontsFolder = Environment.GetFolderPath( Environment.SpecialFolder.Fonts );
		public static readonly string localFontsFolder = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ) + "\\Microsoft\\Windows\\Fonts\\";

		public ItemCollection GetValues()
		{
			var typefaceItemCollection = new ItemCollection();

			ScanFolder( globalFontsFolder, typefaceItemCollection );
			ScanFolder( localFontsFolder, typefaceItemCollection );

			typefaceItemCollection.Sort( ( item1, item2 ) => item1.DisplayName.CompareTo( item2.DisplayName ) );

			return typefaceItemCollection;
		}

		public static void ScanFolder( string fontsFolder, ItemCollection typefaceItemCollection )
		{
			var filesInFolder = Directory.EnumerateFiles( fontsFolder );

			foreach ( var filePath in filesInFolder )
			{
				var typeFaces = Fonts.GetTypefaces( filePath );

				if ( typeFaces.Count == 0 )
				{
					continue;
				}

				var typeFace = typeFaces.First();

				var fontName = typeFace.FontFamily.Source.Split( "#" ).Last();

				if ( typeFace.Stretch.ToString() != "Normal" )
				{
					fontName += $" {typeFace.Stretch}";
				}

				if ( typeFace.Style.ToString() != "Normal" )
				{
					fontName += $" {typeFace.Style}";
				}

				if ( typeFace.Weight.ToString() != "Normal" )
				{
					fontName += $" {typeFace.Weight}";
				}

				var item = new Item()
				{
					DisplayName = fontName,
					Value = filePath
				};

				if ( !typefaceItemCollection.Contains( item ) )
				{
					typefaceItemCollection.Add( filePath, fontName );
				}
			}
		}
	}
}
