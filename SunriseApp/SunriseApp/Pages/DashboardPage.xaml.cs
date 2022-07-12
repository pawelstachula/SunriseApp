using System;
using System.Drawing;
using MvvmCross.Forms.Views;
using SunriseApp.PageModels;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SunriseApp.Pages;

public partial class DashboardPage : MvxContentPage<DashboardPageModel>
{
    public DashboardPage()
    {
        InitializeComponent();
        On<iOS>().SetUseSafeArea(true);
        Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
    }

    private void LonEntryCompleted(object sender, EventArgs e)
    {
        LngEntry.Focus();
    }
}