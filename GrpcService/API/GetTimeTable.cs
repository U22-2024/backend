using System.Net.Http.Headers;
using System.Text.Json;
using Event.V1;
using Microsoft.IdentityModel.Tokens;
using DateTime = Event.V1.DateTime;

namespace GrpcService.API;

public class GetTimeTable(IConfiguration _config)
{
    public async Task<List<TimeTable>> GetTimeTableList(EventMaterial eventMaterial, bool isStart)
    {
        if (!eventMaterial.IsOut)
            return new List<TimeTable>();

        switch (eventMaterial.MoveType)
        {
            case MoveType.Train:
                return await GetTimeTableListByNaviTime(eventMaterial, isStart);
            case MoveType.Car:
            case MoveType.Walk:
                return await GetTimeTableListByGoogle(eventMaterial, isStart);
            default:
                return new List<TimeTable>();
        }
    }

    private async Task<List<TimeTable>> GetTimeTableListByNaviTime(EventMaterial eventMaterial, bool isStart)
    {
        string timeStr = isStart ? "goal_time=" + GetDateTimeString(eventMaterial.StartTime) : "start_time=" + GetDateTimeString(eventMaterial.EndTime);
        string order = "";
        if (!eventMaterial.Option.IsNullOrEmpty())
            order = "&order=" + eventMaterial.Option;

        string url = "https://navitime-route-totalnavi.p.rapidapi.com/route_transit?start=" + eventMaterial.FromPos.Lat +
                     "%2C" + eventMaterial.FromPos.Lon + "&goal=" + eventMaterial.DestinationPos.Lat + "%2C" + eventMaterial.DestinationPos.Lon +
                     "&datum=wgs84&term=1440" + order + "&limit=5&" + timeStr + "&coord_unit=degree";

        Console.WriteLine(url);

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
            {
                { "x-rapidapi-key", _config["RapidApiKey"] },
                { "x-rapidapi-host", "navitime-route-totalnavi.p.rapidapi.com" },
            },
        };

        NaviTimeFormat? naviTimeFormat;
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            string body = await response.Content.ReadAsStringAsync();

            naviTimeFormat = JsonSerializer.Deserialize<NaviTimeFormat>(body);
        }

        List<TimeTable> timeTableList = new List<TimeTable>();
        foreach (var item in naviTimeFormat.items)
        {
            TimeTable timeTable = new TimeTable();
            timeTable.TransitCount = (uint)item.summary.move.transit_count;
            timeTable.WalkDistance = (uint)item.summary.move.walk_distance;
            timeTable.Fare = (uint)item.summary.move.fare.unit_0;

            timeTable.Item.Clear();
            foreach (var section in item.sections)
            {
                TimeTableItem timeTableItem = new TimeTableItem();

                TimeTableType type;
                switch (section.type)
                {
                    case "point":
                        type = TimeTableType.Point;
                        break;
                    case "move":
                        type = TimeTableType.Move;
                        break;
                    default:
                        type = TimeTableType.Unspecified;
                        break;
                }
                timeTableItem.Type = type;

                if (type == TimeTableType.Point)
                {
                    string name = section.name;
                    if (name == "start") name = "自宅";
                    if (name == "goal") name = eventMaterial.Destination;
                    if (section.node_types.Contains("station")) name += "駅";
                    if (section.gateway != "") name += " " + section.gateway;
                    timeTableItem.Name = name;
                }

                if (type == TimeTableType.Move)
                {
                    timeTableItem.Move = GetMoveType(section.move);
                    timeTableItem.FromTime = GetDateTime(section.from_time);
                    timeTableItem.EndTime = GetDateTime(section.to_time);
                    timeTableItem.Distance = (uint)section.distance;
                    timeTableItem.LineName = section.line_name;

                    if (timeTableItem.Move == "train" || timeTableItem.Move == "paid_train")
                    {
                        var transport = new Event.V1.Transport();
                        transport.Fare = (uint)section.transport.fare.unit_0;
                        transport.Color = section.transport.color;
                        transport.TrainName = section.transport.self_name;
                        transport.Direction = section.transport.links[0].direction;
                        transport.Destination = section.transport.links[0].destination.name;
                        timeTableItem.Transport = transport;
                    }
                }

                timeTable.Item.Add(timeTableItem);
            }

            timeTableList.Add(timeTable);
        }

        return timeTableList;
    }

    private string GetDateTimeString(DateTime dateTime)
    {
        return dateTime.Year.ToString("D4") + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") +
               "T" + dateTime.Hour.ToString("D2") + "%3A" + dateTime.Minute.ToString("D2") + "%3A00";
    }

    private DateTime GetDateTime(string dateTimeStr)
    {
        DateTime dateTime = new DateTime();
        if (dateTimeStr == "")
            return dateTime;

        string[] dateTimeArray = dateTimeStr.Split('T');
        string[] dateArray = dateTimeArray[0].Split('-');
        string[] timeArray = dateTimeArray[1].Split(':');
        dateTime.Year = uint.Parse(dateArray[0]);
        dateTime.Month = uint.Parse(dateArray[1]);
        dateTime.Day = uint.Parse(dateArray[2]);
        dateTime.Hour = uint.Parse(timeArray[0]);
        dateTime.Minute = uint.Parse(timeArray[1]);
        return dateTime;
    }

    private string GetMoveType(string text)
    {
        string move;
        switch (text)
        {
            case "walk":
                move = "walk";
                break;
            case "sleeper_ultraexpress":
            case "ultraexpress_train":
            case "express_train":
            case "semiexpress_train":
                // 追加料金電車
                move = "paid_train";
                break;
            case "rapid_train":
            case "local_train":
                // 運賃のみ電車
                move = "train";
                break;
            case "car":
                move = "car";
                break;
            default:
                move = "other";
                break;
        }

        return move;
    }

    private async Task<List<TimeTable>> GetTimeTableListByGoogle(EventMaterial eventMaterial, bool isStart)
    {
        // 力尽きた
        return new List<TimeTable>();
    }
}

public class NaviTimeFormat
{
    public List<Route> items { get; set; } = new List<Route>();
}

public class Route
{
    public RouteSummary summary { get; set; } = new RouteSummary();
    public List<RouteSectionItem> sections { get; set; } = new List<RouteSectionItem>();
}

public class RouteSummary
{
    public RouteSummaryItem move { get; set; } = new RouteSummaryItem();
}

public class RouteSummaryItem
{
    public int transit_count { get; set; } = -1;
    public int walk_distance { get; set; } = -1;
    public Fare fare { get; set; } = new Fare();
}

public class Fare
{
    public double unit_0 { get; set; } = -1;
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
    public Transport transport { get; set; } = new Transport();
    public string from_time { get; set; } = "";
    public string to_time { get; set; } = "";
    public int distance { get; set; } = -1;
    public string line_name { get; set; } = "";
}

public class Transport
{
    public Fare fare { get; set; } = new Fare();
    public string color { get; set; } = "";
    public string self_name { get; set; } = "";
    public List<Link> links { get; set; } = new List<Link>();
}

public class Link
{
    public string direction { get; set; } = "";
    public NodeItem destination { get; set; } = new NodeItem();
}

public class NodeItem
{
    public string name { get; set; } = "";
}
