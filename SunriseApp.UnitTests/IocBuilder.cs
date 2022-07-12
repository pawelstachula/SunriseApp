using System.Collections.Generic;
using MvvmCross.IoC;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace SunriseApp.UnitTests;

public class IocBuilder
{
    private readonly IMvxIoCProvider _ioc;
    private readonly List<object> _mocks = new();

    public IocBuilder(IMvxIoCProvider ioc)
    {
        _ioc = ioc;
    }

    public T Mock<T>() where T : class
    {
        var mock = Substitute.For<T>();
            
        _ioc.RegisterSingleton(mock);
        _mocks.Add(mock);

        return mock;
    }

    public void ClearCallsAndMocks()
    {
        _mocks.ForEach(mock => mock.ClearSubstitute());
    }
}