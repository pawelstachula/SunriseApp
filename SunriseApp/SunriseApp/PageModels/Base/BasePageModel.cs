using MvvmCross.ViewModels;
using SunriseApp.Services.Interfaces;

namespace SunriseApp.PageModels.Base;

public class BasePageModel : MvxViewModel
{
    protected ILogger Logger { get; }
    protected IDialogService DialogService { get; }
    protected IConnectivityService ConnectivityService { get; }

    public BasePageModel(
        ILogger logger,
        IDialogService dialogService,
        IConnectivityService connectivityService)
    {
        Logger = logger;
        DialogService = dialogService;
        ConnectivityService = connectivityService;
    }
}