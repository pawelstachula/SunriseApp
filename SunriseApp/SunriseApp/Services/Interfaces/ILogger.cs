using System;

namespace SunriseApp.Services.Interfaces;

public interface ILogger
{
    void LogError(Exception ex);
    void LogDebugMessage(string message);
}