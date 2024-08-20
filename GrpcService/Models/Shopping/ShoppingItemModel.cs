using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shopping.V1;

namespace GrpcService.Models.Shopping;

public class ShoppingItemModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [MaxLength(128)] public required string Name { get; set; }

    [MaxLength(128)] public string? Quantity { get; set; }

    [MaxLength(128)] public required string Type { get; set; }

    [MaxLength(128)] public string? Memo { get; set; }

    public required ItemStatus Status { get; set; }

    [MaxLength(32)] public required string Uid { get; init; }
}
