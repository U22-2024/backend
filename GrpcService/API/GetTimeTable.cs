using System.Text.Json;
using Event.V1;
using Microsoft.IdentityModel.Tokens;
using DateTime = Event.V1.DateTime;

namespace GrpcService.API;

public class GetTimeTable(IConfiguration config)
{
    public async Task<List<TimeTable>> GetTimeTableList(EventMaterial eventMaterial, bool isStart)
    {
        if (!eventMaterial.IsOut)
            return [];

        return eventMaterial.MoveType switch
        {
            MoveType.Train => await GetTimeTableListByNaviTime(eventMaterial, isStart),
            MoveType.Car or MoveType.Walk => await GetTimeTableListByGoogle(eventMaterial, isStart),
            _ => []
        };
    }

    private async Task<List<TimeTable>> GetTimeTableListByNaviTime(EventMaterial eventMaterial, bool isStart)
    {
        var timeStr = isStart
            ? "goal_time=" + GetDateTimeString(eventMaterial.StartTime)
            : "start_time=" + GetDateTimeString(eventMaterial.EndTime);
        var order = "";
        if (!eventMaterial.Option.IsNullOrEmpty())
            order = "&order=" + eventMaterial.Option;

        var url = "https://navitime-route-totalnavi.p.rapidapi.com/route_transit?start=" + eventMaterial.FromPos.Lat +
                  "%2C" + eventMaterial.FromPos.Lon + "&goal=" + eventMaterial.DestinationPos.Lat + "%2C" +
                  eventMaterial.DestinationPos.Lon +
                  "&datum=wgs84&term=1440" + order + "&limit=5&" + timeStr + "&coord_unit=degree";

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
            {
                { "x-rapidapi-key", config["RapidApiKey"] },
                { "x-rapidapi-host", "navitime-route-totalnavi.p.rapidapi.com" }
            }
        };

        NaviTimeFormat? naviTimeFormat;
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            naviTimeFormat = JsonSerializer.Deserialize<NaviTimeFormat>(body);
            if (naviTimeFormat == null)
                return [];
        }

        var timeTableList = new List<TimeTable>();
        foreach (var item in naviTimeFormat.items)
        {
            var timeTable = new TimeTable
            {
                TransitCount = (uint)item.summary.move.transit_count,
                WalkDistance = (uint)item.summary.move.walk_distance,
                Fare = (uint)item.summary.move.fare.unit_0
            };

            timeTable.Item.Clear();
            foreach (var section in item.sections)
            {
                var timeTableItem = new TimeTableItem();

                var type = section.type switch
                {
                    "point" => TimeTableType.Point,
                    "move" => TimeTableType.Move,
                    _ => TimeTableType.Unspecified
                };

                timeTableItem.Type = type;

                switch (type)
                {
                    case TimeTableType.Point:
                    {
                        var name = section.name;
                        if (name == "start") name = "自宅";
                        if (name == "goal") name = eventMaterial.Destination;
                        if (section.node_types.Contains("station")) name += "駅";
                        if (section.gateway != "") name += " " + section.gateway;
                        timeTableItem.Name = name;
                        break;
                    }
                    case TimeTableType.Move:
                    {
                        timeTableItem.Move = GetMoveType(section.move);
                        timeTableItem.FromTime = GetDateTime(section.from_time);
                        timeTableItem.EndTime = GetDateTime(section.to_time);
                        timeTableItem.Distance = (uint)section.distance;
                        timeTableItem.LineName = section.line_name;

                        var transport = new Event.V1.Transport
                        {
                            Fare = (uint)section.transport.fare.unit_0,
                            Color = section.transport.color,
                            TrainName = section.transport.self_name,
                            Direction = section.transport.links[0].direction,
                            Destination = section.transport.links[0].destination.name
                        };
                        timeTableItem.Transport = transport;
                        break;
                    }
                    case TimeTableType.Unspecified:
                        break;
                    default:
                        throw new Exception("Unknown type");
                }

                timeTable.Item.Add(timeTableItem);
            }

            timeTableList.Add(timeTable);
        }

        return timeTableList;
    }

    private static string GetDateTimeString(DateTime dateTime)
    {
        return dateTime.Year.ToString("D4") + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") +
               "T" + dateTime.Hour.ToString("D2") + "%3A" + dateTime.Minute.ToString("D2") + "%3A00";
    }

    private static DateTime GetDateTime(string dateTimeStr)
    {
        var dateTime = new DateTime();
        if (dateTimeStr == "")
            return dateTime;

        var dateTimeArray = dateTimeStr.Split('T');
        var dateArray = dateTimeArray[0].Split('-');
        var timeArray = dateTimeArray[1].Split(':');
        dateTime.Year = uint.Parse(dateArray[0]);
        dateTime.Month = uint.Parse(dateArray[1]);
        dateTime.Day = uint.Parse(dateArray[2]);
        dateTime.Hour = uint.Parse(timeArray[0]);
        dateTime.Minute = uint.Parse(timeArray[1]);
        return dateTime;
    }

    private static string GetMoveType(string text)
    {
        var move = text switch
        {
            "walk" => "walk",
            "sleeper_ultraexpress" or "ultraexpress_train" or "express_train" or "semiexpress_train" =>
                // 追加料金電車
                "paid_train",
            "rapid_train" or "local_train" =>
                // 運賃のみ電車
                "train",
            "car" => "car",
            _ => "other"
        };

        return move;
    }

    private Task<List<TimeTable>> GetTimeTableListByGoogle(EventMaterial eventMaterial, bool isStart)
    {
        // 力尽きた
        return Task.FromResult(new List<TimeTable>());
    }
}

public class NaviTimeFormat
{
    public List<Route> items { get; set; } = new();
}

public class Route
{
    public RouteSummary summary { get; set; } = new();
    public List<RouteSectionItem> sections { get; set; } = new();
}

public class RouteSummary
{
    public RouteSummaryItem move { get; set; } = new();
}

public class RouteSummaryItem
{
    public int transit_count { get; set; } = -1;
    public int walk_distance { get; set; } = -1;
    public Fare fare { get; set; } = new();
}

public class Fare
{
    public int unit_0 { get; set; } = -1;
}

public class RouteSectionItem
{
    public string type { get; set; } = "";

    // type = "point"
    public string name { get; set; } = "";
    public string[] node_types { get; set; } = Array.Empty<string>();
    public string gateway { get; set; } = "";

    // type = "move"
    public string move { get; set; } = "";
    public Transport transport { get; set; } = new();
    public string from_time { get; set; } = "";
    public string to_time { get; set; } = "";
    public int distance { get; set; } = -1;
    public string line_name { get; set; } = "";
}

public class Transport
{
    public Fare fare { get; set; } = new();
    public string color { get; set; } = "";
    public string self_name { get; set; } = "";
    public List<Link> links { get; set; } = new();
}

public class Link
{
    public string direction { get; set; } = "";
    public NodeItem destination { get; set; } = new();
}

public class NodeItem
{
    public string name { get; set; } = "";
}
