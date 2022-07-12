using MvvmCross.Forms.Converters;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SunriseApp.Converters;

public class CheckBoxBackgroundColorConverter : MvxFormsValueConverter<bool, Color>
{
    protected override Color Convert(bool value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true ? (Color) App.Current.Resources["PrimaryColor"]
            : (Color) App.Current.Resources["White"];;
    }

    protected override bool ConvertBack(Color value, Type targetType, object parameter, CultureInfo culture)
    {
        return false;
    }
}