namespace SunriseApp.Utils;

public static class RegexPatterns
{
    public static string CITY_NAME_PATTERN = @"^[a-zA-Z\s]*$";
    public static string COORDINATES_PATTERN = @"^((\-?|\+?)?\d+(\.\d+)?)$";
}