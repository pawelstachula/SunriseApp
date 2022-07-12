using Android.Content;
using Android.Content.Res;
using SunriseApp.Controls;
using SunriseApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]  
namespace SunriseApp.Droid.Renderers  
{  
    public class CustomEntryRenderer: EntryRenderer  
    {  
        public CustomEntryRenderer(Context context) : base(context)  
        {  
            AutoPackage = false;  
        }  
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)  
        {  
            base.OnElementChanged(e);  
            if (Control != null)  
            {  
                var entryLineColor = Android.Graphics.Color.Transparent;
                Control.BackgroundTintList = ColorStateList.ValueOf(entryLineColor);
            }  
        }  
    }  
}