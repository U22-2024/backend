using System.ComponentModel.DataAnnotations;

namespace GrpcService.Models;

public class User
{
    [Key] [MaxLength(36)] public required string Uid { get; init; }
}
