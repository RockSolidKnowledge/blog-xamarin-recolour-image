using System.IO;
using Xamarin.Forms;

namespace RecolorImageExample
{
	public interface IPlatformResourceImageResolver
	{
		Stream GetImageData(FileImageSource source);
	}
}