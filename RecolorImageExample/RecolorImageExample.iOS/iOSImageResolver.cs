using System;
using System.IO;
using UIKit;
using Xamarin.Forms;

namespace RecolorImageExample.iOS
{
	class iOSImageResolver : IPlatformResourceImageResolver
	{
		public Stream GetImageData(FileImageSource source)
		{
			var filesource = source;
			var file = filesource?.File;
			if (!string.IsNullOrEmpty(file))
			{
				var image = File.Exists(file) ? new UIImage(file) : UIImage.FromBundle(file);
				return image.AsPNG().AsStream();
			}

			throw new ArgumentException("Image file did not exist");
		}
	}
}