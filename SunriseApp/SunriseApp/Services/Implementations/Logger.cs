using System;
using System.Diagnostics;
using SunriseApp.Services.Interfaces;

namespace SunriseApp.Services.Implementations;

public class Logger : ILogger
{
    public void LogError(Exception ex)
    {
        Debug.WriteLine(new string('*', 100));
        Debug.WriteLine(ex.Message);
        Debug.WriteLine(ex.StackTrace);
        Debug.WriteLine(new string('*', 100));
    }

    public void LogDebugMessage(string message)
    {
        Debug.WriteLine($"DEBUG LOG: {message} {DateTime.Now.ToShortTimeString()}");
    }
}