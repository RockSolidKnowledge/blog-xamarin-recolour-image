using TinyIoC;
using Xamarin.Forms;

namespace RecolorImageExample
{
	class ColorTransformImage : Image
	{
		private IColorTransformService _transformer;

		private IColorTransformService Transformer
		{
			get
			{
				return _transformer ?? (_transformer = TinyIoCContainer.Current.Resolve<IColorTransformService>());
			}
		}

		public static readonly BindableProperty SourceImageProperty = BindableProperty.Create(nameof(SourceImage), typeof(FileImageSource), typeof(ColorTransformImage), null, propertyChanged: OnSourceImagePropertyChanged);

		public static readonly BindableProperty SourceImageColorProperty = BindableProperty.Create(nameof(SourceImageColor), typeof(Color), typeof(ColorTransformImage), Color.Default, propertyChanged: OnSourceImageColorPropertyChanged);

		public static readonly BindableProperty TargetTintColorProperty = BindableProperty.Create(nameof(TargetTintColor), typeof(Color), typeof(ColorTransformImage), Color.Default, propertyChanged: OnTargetTintColorPropertyChanged);

		public ImageSource SourceImage
		{
			get => (ImageSource)GetValue(SourceImageProperty);
			set => SetValue(SourceImageProperty, value);
		}

		public Color SourceImageColor
		{
			get => (Color)GetValue(SourceImageColorProperty);
			set => SetValue(SourceImageColorProperty, value);
		}

		public Color TargetTintColor
		{
			get => (Color)GetValue(TargetTintColorProperty);
			set => SetValue(TargetTintColorProperty, value);
		}

		private static void OnSourceImagePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorTransformImage button = (ColorTransformImage)bindable;
			if (CanTransformSourceImage(button))
				button.TransformSourceImage();
		}

		private static void OnSourceImageColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorTransformImage button = (ColorTransformImage)bindable;
			if (CanTransformSourceImage(button))
				button.TransformSourceImage();
		}

		private static void OnTargetTintColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			ColorTransformImage button = (ColorTransformImage)bindable;
			if (CanTransformSourceImage(button))
				button.TransformSourceImage();
		}

		private static bool CanTransformSourceImage(ColorTransformImage button)
		{
			return button.SourceImage != null;
		}

		private void TransformSourceImage()
		{
			var imageSource = (FileImageSource)SourceImage;

			if (SourceImageColor == TargetTintColor || !IsValidForTransformation(TargetTintColor))
			{
				Source = imageSource;
				return;
			}

			this.Source = Transformer.TransformFileImageSource(imageSource, this.TargetTintColor);
		}

		public bool IsValidForTransformation(Color color)
		{
			// Color.Default has negative values for R,G,B,A
			return color.R >= 0 &&
			       color.G >= 0 &&
			       color.B >= 0;
		}
	}
}