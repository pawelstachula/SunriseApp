using System.Threading.Tasks;
using Refit;
using SunriseApp.Models;

namespace SunriseApp.Services.Interfaces;

public interface ISunriseApi
{
    [Get("/json?lat={lat}&lng={lng}")]
    Task<SunsApiRespone> GetSunsetSunriseData(string lat, string lng);
}