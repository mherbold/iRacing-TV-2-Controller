
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace iRacingTV
{
	public class Font
	{
		public string Name { get; set; } = "New Font";
		[ItemsSource( typeof( TypefaceItemsSource ) )]
		public string Typeface { get; set; } = string.Empty;

		public override string ToString()
		{
			return Name;
		}
	}
}
