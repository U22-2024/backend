using System.ComponentModel.DataAnnotations;

namespace GrpcService.Models;

public class User
{
    [Key] [MaxLength(36)] public required string Uid { get; init; }
    [MaxLength(100)] public required string Email { get; init; }
    [MaxLength(100)] public required string DisplayName { get; set; }
    [MaxLength(100)] public required string IconUrl { get; set; }
}