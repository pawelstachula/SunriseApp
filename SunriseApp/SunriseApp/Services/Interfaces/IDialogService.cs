using System.Threading.Tasks;

namespace SunriseApp.Services.Interfaces;

public interface IDialogService
{
    Task ShowAlert(string title);
}
