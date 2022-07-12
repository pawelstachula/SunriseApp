namespace SunriseApp.Models;

public class SunsApiRespone
{
    public SunsModel Results { get; set; }
    public string Status { get; set; }
    public bool DidSucceed => Status is "OK";
}