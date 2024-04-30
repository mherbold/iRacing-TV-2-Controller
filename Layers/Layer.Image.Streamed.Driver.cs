
namespace iRacingTV
{
	internal class DriverStreamedImageLayer : StreamedImageLayer
	{
		public enum ImageType
		{
			CarNumber = 1,
			Car = 2,
			CarLogo = 3,
			Driver = 4,
			Helmet = 5,
			MemberClubRegion = 6,
			MemberID_A = 7,
			MemberID_B = 8,
			MemberID_C = 9,
		}

		public ImageType Type { get; set; } = ImageType.CarNumber;
	}
}
