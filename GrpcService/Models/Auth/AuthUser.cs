namespace GrpcService.Models.Auth;

/// <summary>
///     認証用のユーザー情報
/// </summary>
public readonly struct AuthUser
{
    public required string Uid { get; init; }
    public required string Email { get; init; }
    public required bool EmailVerified { get; init; }
}
