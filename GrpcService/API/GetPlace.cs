using System.Text;
using Event.V1;
using Grpc.Core;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GrpcService.API;

public class GetPlace(IConfiguration config)
{
    public async Task<List<Place>> GetTextPos(string text, Pos homePos)
    {
        var client = new HttpClient();

        var requestBody = new
        {
            textQuery = text,
            pageSize = 5,
            locationBias = new
            {
                circle = new
                {
                    center = new { latitude = homePos.Lat, longitude = homePos.Lon },
                    radius = 50000.0
                }
            },
            languageCode = "ja"
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("X-Goog-Api-Key", config["GoogleApiKey"]);
        client.DefaultRequestHeaders.Add("X-Goog-FieldMask",
            "places.displayName,places.formattedAddress,places.location,nextPageToken");

        var response = await client.PostAsync("https://places.googleapis.com/v1/places:searchText", content);

        var posList = new List<Place>();
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            var placeFormat = JsonSerializer.Deserialize<PlaceFormat>(responseBody);
            if (placeFormat == null)
                throw new RpcException(new Status(StatusCode.Internal, "Google Place API error"));

            posList.AddRange(placeFormat.places.Select(place => new Place
            {
                Name = place.displayName.text, Address = place.formattedAddress,
                Pos = new Pos { Lat = place.location.latitude, Lon = place.location.longitude }
            }));
        }
        else
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Google Place API error | {response.StatusCode}"));
        }

        return posList;
    }
}

public class PlaceFormat
{
    public List<Places> places { get; set; }
    public string nextPageToken { get; set; }
}

public class Places
{
    public string formattedAddress { get; set; }
    public Location location { get; set; }
    public DisplayName displayName { get; set; }
}

public class Location
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}

public class DisplayName
{
    public string text { get; set; }
    public string languageCode { get; set; }
}
