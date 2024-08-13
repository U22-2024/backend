using Grpc.Core;
using GrpcService.Extensions;
using Healthcheck.V1;
using Microsoft.AspNetCore.Authorization;

namespace GrpcService.Services;

public class HealthCheckService: Healthcheck.V1.HealthCheckService.HealthCheckServiceBase
{
    [Authorize]
    public override Task<HealthCheckResponse> HealthCheck(HealthCheckRequest request, ServerCallContext context)
    {
        var user = context.GetAuthUser();

        return Task.FromResult(new HealthCheckResponse
        {
            Status = $"User UID: {user.Uid}"
        });
    }
}
