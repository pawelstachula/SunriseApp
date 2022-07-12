using System;
using MvvmCross.ViewModels;

namespace SunriseApp.Models;

public class DashboardModel : MvxNotifyPropertyChanged
{
    private string _sunrise;
    private string _sunset;
    private string _dayLength;
    
    public string Sunrise
    {
        get => _sunrise;
        set => SetProperty(ref _sunrise, value);
    }

    public string Sunset
    {
        get => _sunset;
        set => SetProperty(ref _sunset, value);
    }

    public string DayLength
    {
        get => _dayLength;
        set => SetProperty(ref _dayLength, value);
    }
    
    public void LoadDataToModel(SunsApiRespone suns)
    {
        Sunrise = suns.Results.Sunrise.ToString("HH:mm:ss");
        Sunset = suns.Results.Sunset.ToString("HH:mm:ss");
        DayLength = $"{suns.Results.DayLength.Hour} hours " +
                          $"{suns.Results.DayLength.Minute} minutes " +
                          $"{suns.Results.DayLength.Second} seconds";
    }

    public void ClearModelData()
    {
        Sunrise = string.Empty;
        Sunset = string.Empty;
        DayLength = string.Empty;
    }
}