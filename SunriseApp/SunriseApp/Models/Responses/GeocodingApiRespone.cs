using System.Collections.Generic;

namespace SunriseApp.Models;

public class GeocodingApiRespone
{
    public List<Result> Results { get; set; }
    public string Status { get; set; }
    public bool DidSucceed => Status is "OK";
}

public class Result
{
    public Geometry Geometry { get; set; }
}