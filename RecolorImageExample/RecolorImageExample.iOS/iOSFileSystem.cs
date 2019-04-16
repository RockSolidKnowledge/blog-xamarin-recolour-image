using System;
using System.IO;
using UIKit;

namespace RecolorImageExample.iOS
{
	class iOSFileSystem : IFileSystem
	{
		public string GetTempImageFilePathForCurrentPixelDensity()
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var tempFolder = Path.Combine(documents, "Temp");

			Directory.CreateDirectory(tempFolder);

			string imageFileName = GetImageDensityFileName();
			var path = Path.Combine(tempFolder, imageFileName);

			return path;
		}

		private string GetImageDensityFileName()
		{
			nfloat scale = UIScreen.MainScreen.Scale;

			int truncated = (int)(scale);

			if (truncated < 2) return Path.GetRandomFileName() + ".png";

			if (truncated == 2) return Path.GetRandomFileName() + "@2x.png";

			return Path.GetRandomFileName() + "@3x.png";
		}
	}
}