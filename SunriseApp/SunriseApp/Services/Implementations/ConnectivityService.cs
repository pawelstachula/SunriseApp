using SunriseApp.Services.Interfaces;
using Xamarin.Essentials;

namespace SunriseApp.Services.Implementations;

public class ConnectivityService : IConnectivityService
{
    public bool HasInternetConnection()
    {
        return Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}