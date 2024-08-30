using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Event.V1;
using DateTime = System.DateTime;

namespace GrpcService.Models.Event;

public class TimeTableItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Required] public required int SeqId { get; set; }

    [Required] public required int Type { get; set; }

    [MaxLength(100)] public string? Name { get; set; }

    [MaxLength(100)] public string? Move { get; set; }

    public DateTime FromTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Distance { get; set; } = -1;

    [MaxLength(100)] public string? LineName { get; set; }

    public int Fare { get; set; } = -1;

    [MaxLength(100)] public string? TrainName { get; set; }

    [MaxLength(10)] public string? Color { get; set; }

    [MaxLength(100)] public string? Direction { get; set; }

    [MaxLength(100)] public string? Destination { get; set; }

    public Guid EventId { get; set; }

    public EventModel Event { get; init; } = null!;
}

public static class TimeTableItemModelExt
{
    public static TimeTableItem ToTimeTableItem(this TimeTableItemModel self)
    {
        if (self.Type == (int)TimeTableType.Point)
            return new TimeTableItem
            {
                Type = TimeTableType.Point,
                Name = self.Name
            };

        if (self.Move == "train")
            return new TimeTableItem
            {
                Type = (TimeTableType)self.Type,
                Name = self.Name,
                Move = self.Move,
                FromTime = new global::Event.V1.DateTime
                {
                    Year = (uint)self.FromTime.Year,
                    Month = (uint)self.FromTime.Month,
                    Day = (uint)self.FromTime.Day,
                    Hour = (uint)self.FromTime.Hour,
                    Minute = (uint)self.FromTime.Minute
                },
                EndTime = new global::Event.V1.DateTime
                {
                    Year = (uint)self.EndTime.Year,
                    Month = (uint)self.EndTime.Month,
                    Day = (uint)self.EndTime.Day,
                    Hour = (uint)self.EndTime.Hour,
                    Minute = (uint)self.EndTime.Minute
                },
                Distance = (uint)self.Distance,
                LineName = self.LineName,
                Transport = new Transport
                {
                    Fare = (uint)self.Fare,
                    TrainName = self.TrainName,
                    Color = self.Color,
                    Direction = self.Direction,
                    Destination = self.Destination
                }
            };

        return new TimeTableItem
        {
            Type = (TimeTableType)self.Type,
            Name = self.Name,
            Move = self.Move,
            FromTime = new global::Event.V1.DateTime
            {
                Year = (uint)self.FromTime.Year,
                Month = (uint)self.FromTime.Month,
                Day = (uint)self.FromTime.Day,
                Hour = (uint)self.FromTime.Hour,
                Minute = (uint)self.FromTime.Minute
            },
            EndTime = new global::Event.V1.DateTime
            {
                Year = (uint)self.EndTime.Year,
                Month = (uint)self.EndTime.Month,
                Day = (uint)self.EndTime.Day,
                Hour = (uint)self.EndTime.Hour,
                Minute = (uint)self.EndTime.Minute
            },
            Distance = (uint)self.Distance,
            LineName = self.LineName
        };
    }
}
