using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;

namespace SunriseApp.Droid;

[Activity(Label = "SunriseApp", 
    Theme = "@style/MainTheme", 
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize 
                           | ConfigChanges.Orientation 
                           | ConfigChanges.UiMode 
                           | ConfigChanges.ScreenLayout 
                           | ConfigChanges.SmallestScreenSize )]
public class MainActivity : MvxFormsAppCompatActivity<MvxFormsAndroidSetup<CoreApp, App>, CoreApp, App>
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
            
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
    }
        
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}