using System.Security.Claims;
using Grpc.Core;

namespace GrpcService.Extensions;

public static class ServerCallContextExt
{
    public static User.V1.User GetAuthUser(this ServerCallContext self, ILogger? logger = null)
    {
        var user = self.GetHttpContext().User;
        var uid = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        var emailVerified = user
            .Claims
            .FirstOrDefault(claim => claim is { ValueType: ClaimValueTypes.Boolean, Type: "email_verified" })
            ?.Value;

        if (uid is null || email is null || emailVerified is null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "User not authenticated"));

        return new User.V1.User
        {
            Uid = uid
        };
    }
}
