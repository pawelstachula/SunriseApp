using System.Threading.Tasks;
using SunriseApp.Resources;
using SunriseApp.Services.Interfaces;
using Xamarin.Forms;

namespace SunriseApp.Services.Implementations;

public class DialogService : IDialogService
{
    private readonly Application _application;

    public DialogService(Application application)
    {
        _application = application;
    }

    public Task ShowAlert(string title) 
        => _application.MainPage.DisplayAlert(title, null, AppResources.OK);
}