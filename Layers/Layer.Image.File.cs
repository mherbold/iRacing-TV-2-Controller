
using System.ComponentModel;

namespace iRacingTV
{
	internal class FileImageLayer : ImageLayer
	{
		[Editor( typeof( PropertyGridFilePicker ), typeof( PropertyGridFilePicker ) )]
		public string FilePath { get; set; } = string.Empty;
	}
}
