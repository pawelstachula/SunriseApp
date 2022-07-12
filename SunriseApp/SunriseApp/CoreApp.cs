using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using SunriseApp.PageModels;
using SunriseApp.Services.Implementations;
using SunriseApp.Services.Interfaces;
using SunriseApp.Utils;
using Xamarin.Forms;

namespace SunriseApp;

public class CoreApp : MvxApplication
{
    public override void Initialize()
    {
        RegisterAppStart<DashboardPageModel>();

        var jsonSerializer = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore,
        };

        var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer(jsonSerializer));
        
        Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILogger, Logger>();
        Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IConnectivityService, ConnectivityService>();
        Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDialogService>(() => new DialogService(Application.Current));

        Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISunriseApi>(() =>
            RestService.For<ISunriseApi>(WebAddress.SUNSET_SUNRISE_URL, refitSettings));
        
        Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IGeocodingApi>(() =>
            RestService.For<IGeocodingApi>(WebAddress.GEOCODING_URL, refitSettings));
    }
}