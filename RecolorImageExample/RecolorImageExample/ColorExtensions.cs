using System;
using SkiaSharp;
using Xamarin.Forms;

namespace RecolorImageExample
{
	public static class ColorExtensions
	{
		public static SKColor ToSKColor(this Color color)
		{
			return new SKColor(
				ToByte(color.R),
				ToByte(color.G),
				ToByte(color.B),
				ToByte(color.A));
		}

		private static byte ToByte(double value)
		{
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException($"Expected a value between 0 and 1, but was {value}");

			return (byte)(value * byte.MaxValue);
		}

		public static bool IsValidForTransformation(this Color color)
		{
			return color.R >= 0 &&
			       color.G >= 0 &&
			       color.B >= 0;
		}
	}
}
