using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService.Models.RemindTemplate;

public class RemindTemplateModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(400)] public string? Description { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(32)]
    public required string Uid { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime UsedAt { get; init; }
}
