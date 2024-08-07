using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService.Models.Remind;

public class RemindModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [MinLength(1)] [MaxLength(400)] public string? Description { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(32)]
    public required string Uid { get; init; }

    [Required]
    public Guid RemindGroupId { get; set; }

    public RemindGroupModel RemindGroup { get; init; } = null!;

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
