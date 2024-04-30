
namespace iRacingTV
{
	internal class WeekendStreamedImageLayer : StreamedImageLayer
	{
		public enum ImageType
		{
			SeriesLogo = 1,
			TrackLogo = 2
		}

		public ImageType Type { get; set; } = ImageType.SeriesLogo;
	}
}
