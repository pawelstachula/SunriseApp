using System.Threading.Tasks;
using Refit;
using SunriseApp.Models;

namespace SunriseApp.Services.Interfaces;

public interface IGeocodingApi
{
    [Get("/json?address={address}&key={geoApiKey}")]
    Task<GeocodingApiRespone> GetGeolocation(string address, string geoApiKey);
}