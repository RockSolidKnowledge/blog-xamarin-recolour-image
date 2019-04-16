using System;
using System.IO;

namespace RecolorImageExample.Droid
{
	class DroidFileSystem : IFileSystem
	{
		public string GetTempImageFilePathForCurrentPixelDensity()
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var tempFolder = Path.Combine(documents, "Temp");

			Directory.CreateDirectory(tempFolder);

			string imageFileName = Path.GetRandomFileName() + ".png";
			var path = Path.Combine(tempFolder, imageFileName);

			return path;
		}
	}
}