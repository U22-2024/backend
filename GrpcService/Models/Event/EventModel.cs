using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService.Models.Event;

public class EventModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [MinLength(1)] [MaxLength(400)]
    public string? Description { get; set; }

    public string[] EventItems { get; set; } = [];

    public ICollection<TimeTableItemModel> TimeTableItems { get; } = new List<TimeTableItemModel>();

    public string[] UserItems { get; set; } = [];

    public int TransitCount { get; set; } = -1;
    public int WalkDistance { get; set; } = -1;
    public int Fare = -1;

    [Required]
    public required string Uid { get; set; } = null!;
}
