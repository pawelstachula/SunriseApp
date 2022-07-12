using System;

namespace SunriseApp.Exceptions;

public class ApplicationUnhandledException : Exception
{
    public ApplicationUnhandledException(string message, Exception innerException) : base(message, innerException)
    {
    }
}