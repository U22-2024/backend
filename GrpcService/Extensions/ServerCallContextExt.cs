using System.Security.Claims;
using Grpc.Core;
using GrpcService.Models.Auth;

namespace GrpcService.Extensions;

public static class ServerCallContextExt
{
    public static AuthUser GetAuthUser(this ServerCallContext self, ILogger? logger = null)
    {
        var user = self.GetHttpContext().User;
        var uid = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                  throw new RpcException(new Status(StatusCode.Unauthenticated,
                      $"User not has {ClaimTypes.NameIdentifier}"));
        var email = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value ??
                    throw new RpcException(new Status(StatusCode.Unauthenticated,
                        $"User not has {ClaimTypes.Email}"));
        var emailVerified = user
            .Claims
            .FirstOrDefault(claim => claim is { ValueType: ClaimValueTypes.Boolean, Type: "email_verified" })
            ?.Value ?? throw new RpcException(new Status(StatusCode.Unauthenticated,
            "User not has email_verified"));

        return new AuthUser
        {
            Uid = uid,
            Email = email,
            EmailVerified = bool.Parse(emailVerified)
        };
    }
}
