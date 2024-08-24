using System.Text;
using System.Text.Json;
using Event.V1;

namespace GrpcService.API;

public class GetPlace(IConfiguration _config)
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

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("X-Goog-Api-Key", _config["GoogleApiKey"]);
        client.DefaultRequestHeaders.Add("X-Goog-FieldMask", "places.displayName,places.formattedAddress,places.location,nextPageToken");

        var response = await client.PostAsync("https://places.googleapis.com/v1/places:searchText", content);

        List<Place> posList = new List<Place>();
        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            PlaceFormat? placeFormat = JsonSerializer.Deserialize<PlaceFormat>(responseBody);

            foreach (var place in placeFormat.places)
            {
                var placeItem = new Place()
                {
                    Name = place.displayName.text,
                    Address = place.formattedAddress,
                    Pos = new Pos()
                    {
                        Lat = place.location.latitude,
                        Lon = place.location.longitude
                    }
                };
                posList.Add(placeItem);
            }
        }
        else
        {
            Console.WriteLine($"Error: Google Place API | {response.StatusCode}");
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
