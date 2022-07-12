using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Commands;
using SunriseApp.Models;
using SunriseApp.PageModels.Base;
using SunriseApp.Resources;
using SunriseApp.Services.Interfaces;
using SunriseApp.Utils;

namespace SunriseApp.PageModels;

public class DashboardPageModel : BasePageModel
{
    private readonly ISunriseApi _sunriseApi;
    private readonly IGeocodingApi _geocodingApi;

    private string _city;
    private string _lat;
    private string _lng;
    private bool _isCheckBoxCheck;
    private bool _fetchingData;

    public DashboardPageModel(ISunriseApi sunriseApi,
        IGeocodingApi geocodingApi,
        ILogger logger,
        IDialogService dialogService,
        IConnectivityService connectivityService) : base(logger, dialogService, connectivityService)
    {
        _sunriseApi = sunriseApi;
        _geocodingApi = geocodingApi;

        GetInfoCommand = new MvxAsyncCommand(ExecuteGetInfoCommand, CanExecuteGetInfo);
        CheckedChangedCommand = new MvxCommand(()=> GetInfoCommand.RaiseCanExecuteChanged());
        
        Model = new DashboardModel();
    }

    public DashboardModel Model { get; }

    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value, GetInfoCommand.RaiseCanExecuteChanged);
    }

    public string Lat
    {
        get => _lat;
        set => SetProperty(ref _lat, value, GetInfoCommand.RaiseCanExecuteChanged);
    }

    public string Lng
    {
        get => _lng;
        set => SetProperty(ref _lng, value, GetInfoCommand.RaiseCanExecuteChanged);
    }

    public bool FetchingData
    {
        get => _fetchingData;
        set => SetProperty(ref _fetchingData, value, GetInfoCommand.RaiseCanExecuteChanged);
    }

    public bool IsCheckBoxCheck
    {
        get => _isCheckBoxCheck;
        set => SetProperty(ref _isCheckBoxCheck, value);
    }

    public IMvxAsyncCommand GetInfoCommand { get; }
    public IMvxCommand CheckedChangedCommand { get; }

    private async Task ExecuteGetInfoCommand()
    {
        try
        {
            if (ConnectivityService.HasInternetConnection() is false)
            {
                await DialogService.ShowAlert(AppResources.NoInternet);
                Logger.LogDebugMessage($"No internet");
                return;
            }

            if (ValidateInputs() is false)
            {
                await DialogService.ShowAlert(IsCheckBoxCheck
                    ? AppResources.InvalidCoordinates
                    : AppResources.InvalidCity);
                Logger.LogDebugMessage($"Input invalid {(IsCheckBoxCheck ? Lat + " " + Lng : City)}");
                return;
            }

            Logger.LogDebugMessage($"Input valid {(IsCheckBoxCheck ? Lat + " " + Lng : City)}");

            Model.ClearModelData();

            _ = IsCheckBoxCheck ? FetchSunsetSunriseData(Lat, Lng) : FetchGeolocationData();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            await DialogService.ShowAlert(AppResources.GeneralError);
        }
    }

    private Task FetchSunsetSunriseData(string lat, string lng)
    {
        return RunApiFunction(async () =>
        {
            var suns = await _sunriseApi.GetSunsetSunriseData(lat, lng);
            switch (suns)
            {
                case { Results: { }, DidSucceed: true }:
                    Model.LoadDataToModel(suns);
                    break;
                case { Results: { }, DidSucceed: false, Status: "INVALID_REQUEST" }:
                    await DialogService.ShowAlert(AppResources.InvalidCoordinates);
                    Logger.LogDebugMessage($"InvalidCoordinates {Lat + " " + Lng}");
                    break;
                default:
                    await DialogService.ShowAlert(AppResources.GeneralError);
                    Logger.LogDebugMessage($"FetchSunsetSunriseData Error");
                    break;
            }

            FetchingData = false;
        });
    }

    private Task FetchGeolocationData()
    {
        return RunApiFunction(async () =>
        {
            var geolocation = await _geocodingApi.GetGeolocation(City, ApiConfiguration.GEO_API_KEY);
            switch (geolocation)
            {
                case { Results: { }, DidSucceed: true }:
                    _ = FetchSunsetSunriseData(geolocation.Results.First().Geometry.Location.Lat.ToString(),
                        geolocation.Results.First().Geometry.Location.Lng.ToString());
                    return;
                case { Results: { }, DidSucceed: false, Status: "INVALID_REQUEST" }:
                case { Results: { }, DidSucceed: false, Status: "ZERO_RESULTS" }:
                    FetchingData = false;
                    await DialogService.ShowAlert(AppResources.CityNotFound);
                    Logger.LogDebugMessage($"CityNotFound {City}");
                    return;
                default:
                    FetchingData = false;
                    await DialogService.ShowAlert(AppResources.GeneralError);
                    Logger.LogDebugMessage($"FetchGeolocationData Error");
                    break;
            }
        });
    }

    private async Task RunApiFunction(Func<Task> apiCall)
    {
        try
        {
            FetchingData = true;
            await apiCall();
        }
        catch (Exception ex)
        {
            FetchingData = false;
            Logger.LogError(ex);
            await DialogService.ShowAlert(AppResources.GeneralError);
        }
    }

    private bool ValidateInputs()
    {
        var pattern = IsCheckBoxCheck ? RegexPatterns.COORDINATES_PATTERN : RegexPatterns.CITY_NAME_PATTERN;
        var rgx = new Regex(pattern);

        return IsCheckBoxCheck ? rgx.IsMatch(Lat) && rgx.IsMatch(Lng) : rgx.IsMatch(City);
    }

    private bool CanExecuteGetInfo()
    {
        if (FetchingData) return false;

        if (IsCheckBoxCheck)
        {
            return !string.IsNullOrWhiteSpace(Lat) && !string.IsNullOrWhiteSpace(Lng);
        }

        return !string.IsNullOrWhiteSpace(City);
    }
}