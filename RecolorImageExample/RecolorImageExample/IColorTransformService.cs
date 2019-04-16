using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using Xamarin.Forms;

namespace RecolorImageExample
{
	interface IColorTransformService
	{
		FileImageSource TransformFileImageSource(FileImageSource source, Color outputColor);
	}

	class ColorTransformService : IColorTransformService
	{
		private readonly IFileSystem _filesystem;
		private readonly IPlatformResourceImageResolver _resolver;
		private readonly TransformedImageCache _cache = new TransformedImageCache();

		public ColorTransformService(IFileSystem filesystem, IPlatformResourceImageResolver resolver)
		{
			_filesystem = filesystem ?? throw new ArgumentNullException(nameof(filesystem));
			_resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
		}

		public FileImageSource TransformFileImageSource(FileImageSource source, Color outputColor)
		{
			string transformedFile = null;

			if (_cache.TryGetValue(source.File, outputColor, out transformedFile))
			{
				return (FileImageSource)ImageSource.FromFile(transformedFile);
			}

			transformedFile = _filesystem.GetTempImageFilePathForCurrentPixelDensity();

			using (Stream sourceImageStream = _resolver.GetImageData(source))
			using (SKBitmap sourceBitmap = SKBitmap.Decode(sourceImageStream))
			{
				SKImageInfo info = new SKImageInfo(
					sourceBitmap.Width,
					sourceBitmap.Height,
					sourceBitmap.ColorType,
					sourceBitmap.AlphaType,
					sourceBitmap.ColorSpace);

				using (SKBitmap outputBitmap = new SKBitmap(info))
				using (SKCanvas canvas = new SKCanvas(outputBitmap))
				using (SKPaint transformationBrush = new SKPaint())
				{
					canvas.DrawColor(SKColors.Transparent);

					var targetColor = outputColor.ToSKColor();

					var tableRed = new byte[256];
					var tableGreen = new byte[256];
					var tableBlue = new byte[256];

					for (int i = 0; i < 256; i++)
					{
						tableRed[i] = targetColor.Red;
						tableGreen[i] = targetColor.Green;
						tableBlue[i] = targetColor.Blue;
					}

					// Alpha channel remains unchanged
					transformationBrush.ColorFilter =
						SKColorFilter.CreateTable(null, tableRed, tableGreen, tableBlue);

					canvas.DrawBitmap(sourceBitmap, info.Rect, transformationBrush);

					using (var image = SKImage.FromBitmap(outputBitmap))
					using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
					using (var stream = new FileStream(transformedFile, FileMode.Create, FileAccess.Write))
						data.SaveTo(stream);
				}
			}

			_cache.Add(source.File, outputColor, transformedFile);

			return (FileImageSource)ImageSource.FromFile(transformedFile);
		}

		private class TransformedImageCache
		{
			private readonly Dictionary<CacheKey, string> _cachedImagesFiles = new Dictionary<CacheKey, string>();

			private class CacheKey : IEquatable<CacheKey>
			{
				public CacheKey(string sourceImageName, Color outputColor)
				{
					SourceImageName = sourceImageName;
					OutputColor = outputColor;
				}

				public string SourceImageName { get; private set; }
				public Color OutputColor { get; private set; }

				public bool Equals(CacheKey other)
				{
					if (ReferenceEquals(null, other)) return false;
					if (ReferenceEquals(this, other)) return true;
					return string.Equals(SourceImageName, other.SourceImageName) && OutputColor.Equals(other.OutputColor);
				}

				public override bool Equals(object obj)
				{
					if (ReferenceEquals(null, obj)) return false;
					if (ReferenceEquals(this, obj)) return true;
					if (obj.GetType() != this.GetType()) return false;
					return Equals((CacheKey)obj);
				}

				public override int GetHashCode()
				{
					unchecked
					{
						return ((SourceImageName != null ? SourceImageName.GetHashCode() : 0) * 397) ^ OutputColor.GetHashCode();
					}
				}
			}

			public bool TryGetValue(string sourceFile, Color outputColor, out string transformedFile)
			{
				var key = new CacheKey(sourceFile, outputColor);

				return _cachedImagesFiles.TryGetValue(key, out transformedFile);
			}

			public void Add(string sourceFile, Color outputColor, string transformedFile)
			{
				var key = new CacheKey(sourceFile, outputColor);

				_cachedImagesFiles[key] = transformedFile;
			}
		}
	}
}