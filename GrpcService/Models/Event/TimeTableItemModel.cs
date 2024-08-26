using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService.Models.Event;

public class TimeTableItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Required]
    public required int Type { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; } = null;

    [MaxLength(100)]
    public string? Move { get; set; } = null;
    public DateTime FromTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Distance { get; set; } = -1;
    [MaxLength(100)]
    public string? LineName { get; set; } = null;

    public int Fare { get; set; } = -1;
    [MaxLength(100)]
    public string? TrainName { get; set; } = null;
    [MaxLength(10)]
    public string? Color { get; set; } = null;
    [MaxLength(100)]
    public string? Direction { get; set; } = null;
    [MaxLength(100)]
    public string? Destination { get; set; } = null;

    [Required]
    public Guid EventId { get; set; }

    public EventModel Event { get; init; } = null!;
}
