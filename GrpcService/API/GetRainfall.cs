using System.ComponentModel;
using System.Text.Json;

namespace GrpcService.API;

public class GetRainfall(IConfiguration _config)
{
    public async Task<List<RainFall>> GetListRainfall(Location location) {
        var client = new HttpClient();
        var appId = _config["YahooClientId"];
        var res = await client.GetAsync($"https://map.yahooapis.jp/weather/V1/place?output=json&coordinates={location.longitude},{location.latitude}&appid={appId}");
        if (res.IsSuccessStatusCode) {
            var contentJsonString = await res.Content.ReadAsStringAsync();
            var content = JsonSerializer.Deserialize<RainFallFormat>(contentJsonString);

            if (content != null) {
                return content.Feature[0].Property.WeatherList.Weather;
            }
        }
        return [];
    }
}

public class RainFallFormat {
    public int Count {get; set;}
    public int Total {get; set;}
    public int Start {get; set;}
    public int Status {get; set;}
    public double Latency {get; set;}
    public string? Description {get; set;}

    public required List<RainFallData> Feature {get; set;}
}

public class RainFallData {
    public string? Id {get; set;}
    public string? Name {get; set;}
    public Geometry? Geometry {get; set;}
    public required WeatherData Property {get; set;}
}


public class Geometry {
    public string? Type {get; set;}
    public string? Coordinates {get; set;}
}

public class WeatherData {
    public int WeatherAreaCode {get; set;}
    public required RainFalls WeatherList {get; set;}
}

public class RainFalls {
    public required List<RainFall> Weather {get; set;}
}

public class RainFall {
    public required string Type {get; set;}
    public required string Date {get; set;}
    public required double Rainfall {get; set;}
}
