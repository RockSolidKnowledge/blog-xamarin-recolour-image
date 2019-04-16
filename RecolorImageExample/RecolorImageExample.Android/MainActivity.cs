using Android.App;
using Android.Content.PM;
using Android.OS;

namespace RecolorImageExample.Droid
{
    [Activity(Label = "RecolorImageExample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

	        var container = TinyIoC.TinyIoCContainer.Current;
	        container.Register<IPlatformResourceImageResolver, DroidImageResolver>();
	        container.Register<IFileSystem, DroidFileSystem>();

			LoadApplication(new App());
        }
    }
}