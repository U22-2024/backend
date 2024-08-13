using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService.Models.Remind;

public class RemindGroupModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Required] [MaxLength(50)] public required string Title { get; set; }

    [MaxLength(400)] public string? Description { get; set; }

    [Required] [MaxLength(32)] public required string Uid { get; init; }

    [Required] public required int IconCodePoint { get; set; }
    [Required] [MaxLength(32)] public required string IconFontFamily { get; set; }

    public ICollection<RemindModel> Reminds { get; } = new List<RemindModel>();

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
