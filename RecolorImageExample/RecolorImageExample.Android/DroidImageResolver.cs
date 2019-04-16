using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms;

namespace RecolorImageExample.Droid
{
	class DroidImageResolver : IPlatformResourceImageResolver
	{
		private readonly Context _context;

		public DroidImageResolver(Context context)
		{
			_context = context;
		}

		public Stream GetImageData(FileImageSource source)
		{
			string file = source?.File;
			if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException($"Expected a file image source, but no file was specified");

			var imageId = _context.Resources.GetIdentifier(file, "drawable", _context.PackageName);

			using (var drawable = _context.GetDrawable(imageId))
			{
				Bitmap bitmap = ((BitmapDrawable)drawable).Bitmap;

				Stream memoryStream = new MemoryStream();
				bitmap.Compress(Bitmap.CompressFormat.Png, quality: 100, stream: memoryStream);
				memoryStream.Seek(0, SeekOrigin.Begin);

				return memoryStream;
			}
		}
	}
}