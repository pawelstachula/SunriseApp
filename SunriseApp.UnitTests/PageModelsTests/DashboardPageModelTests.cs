using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using SunriseApp.Models;
using SunriseApp.PageModels;
using SunriseApp.Resources;

namespace SunriseApp.UnitTests.PageModelsTests;

public class DashboardPageModelTests : IocSetup
{
    [Test]
    public void EnsureModelCreated()
    {
        var vm = Ioc.IoCConstruct<DashboardPageModel>();

        vm.Model.Should().NotBeNull();
    }

    [Test]
    public void GetInfoCommand_ShouldDisplayError_WhenNoInternetConnection()
    {
        ConnectivityService.HasInternetConnection().Returns(false);
        var vm = Ioc.IoCConstruct<DashboardPageModel>();

        vm.City = "Paris";
        vm.IsCheckBoxCheck = false;
        vm.GetInfoCommand.Execute();

        using (new AssertionScope())
        {
            ConnectivityService.Received(1).HasInternetConnection();
            DialogService.Received(1).ShowAlert(AppResources.NoInternet);
            Logger.DidNotReceive().LogError(Arg.Any<Exception>());
            Logger.Received().LogDebugMessage("No internet");
        }
    }

    [TestCase("123")]
    [TestCase("a.b")]
    [TestCase("abc 123")]
    [TestCase("Par1s")]
    public void GetInfoCommand_ShouldDisplayError_WhenInvalidCity(string city)
    {
        ConnectivityService.HasInternetConnection().Returns(true);
        var vm = Ioc.IoCConstruct<DashboardPageModel>();

        vm.City = city;
        vm.IsCheckBoxCheck = false;
        vm.GetInfoCommand.Execute();

        using (new AssertionScope())
        {
            ConnectivityService.Received(1).HasInternetConnection();
            DialogService.Received(1).ShowAlert(AppResources.InvalidCity);
            Logger.DidNotReceive().LogError(Arg.Any<Exception>());
            Logger.Received().LogDebugMessage($"Input invalid {city}");
        }
    }

    [TestCase("12", "ab")]
    [TestCase("1-2", "13")]
    [TestCase("12.45", "32.b")]
    [TestCase("12,45", "32")]
    public void GetInfoCommand_ShouldDisplayError_WhenInvalidCoordinates(string lat, string lng)
    {
        ConnectivityService.HasInternetConnection().Returns(true);
        var vm = Ioc.IoCConstruct<DashboardPageModel>();

        vm.Lat = lat;
        vm.Lng = lng;
        vm.IsCheckBoxCheck = true;
        vm.GetInfoCommand.Execute();

        using (new AssertionScope())
        {
            ConnectivityService.Received(1).HasInternetConnection();
            DialogService.Received(1).ShowAlert(AppResources.InvalidCoordinates);
            Logger.DidNotReceive().LogError(Arg.Any<Exception>());
            Logger.Received().LogDebugMessage($"Input invalid {lat + " " + lng}");
        }
    }

    [Test]
    public void GetInfoCommand_ShouldDisplayError_WhenAllArgsCorrect()
    {
        ConnectivityService.HasInternetConnection().Returns(true);
        GeocodingApi.GetGeolocation(Arg.Any<string>(), Arg.Any<string>()).Returns(
            new GeocodingApiRespone
            {
                Results = new List<Result>()
                {
                    new()
                    {
                        Geometry = new Geometry()
                        {
                            Location = new Location()
                            {
                                Lat = 20,
                                Lng = 20
                            }
                        }
                    }
                },
                Status = "OK"
            });
        SunriseApi.GetSunsetSunriseData(Arg.Any<string>(), Arg.Any<string>()).Returns(
            new SunsApiRespone
            {
                Results = new SunsModel()
                {
                    DayLength = DateTime.Now,
                    Sunrise = DateTime.Now,
                    Sunset = DateTime.Now
                },
                Status = "OK"
            });
        var vm = Ioc.IoCConstruct<DashboardPageModel>();

        vm.City = "Paris";
        vm.Lat = "12";
        vm.Lng = "12";
        vm.IsCheckBoxCheck = false;
        vm.GetInfoCommand.Execute();

        using (new AssertionScope())
        {
            ConnectivityService.Received(1).HasInternetConnection();
            DialogService.DidNotReceive().ShowAlert(Arg.Any<string>());
            Logger.DidNotReceive().LogError(Arg.Any<Exception>());
            Logger.Received().LogDebugMessage($"Input valid Paris");
            vm.Lat.Should().NotBeEmpty();
            vm.Lng.Should().NotBeEmpty();
            vm.Model.Sunrise.Should().NotBeEmpty();
            vm.Model.Sunset.Should().NotBeEmpty();
            vm.Model.DayLength.Should().NotBeEmpty();
        }
    }
}