using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService.Models.Greet;

public class GreetModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public required string Message { get; set; }
}
