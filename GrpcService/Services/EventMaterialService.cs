using Event.V1;
using Grpc.Core;
using GrpcService.API;
using GrpcService.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace GrpcService.Services;

public class EventMaterialService(AppDbContext dbContext, ILogger<EventMaterialService> logger)
    : Event.V1.EventMaterialService.EventMaterialServiceBase
{
    [Authorize]
    public override async Task<PredictEventMaterialItemResponse> PredictEventMaterialItem(PredictEventMaterialItemRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (request.Uid.Value != authUser.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var prompt = request.Text;
        var eventMaterial = request.EventMaterial;

        var scope = context.GetHttpContext().RequestServices.CreateScope();

        PredictEventMaterial predictEventMaterial = scope.ServiceProvider.GetRequiredService<PredictEventMaterial>();
        var after = await predictEventMaterial.UpdateEventMaterial(prompt, eventMaterial);
        return new PredictEventMaterialItemResponse
        {
            EventMaterial = after
        };
    }

    [Authorize]
    public override async Task<PredictPositionsFromTextResponse> PredictPositionsFromText(PredictPositionsFromTextRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (request.Uid.Value != authUser.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var prompt = request.Text;
        var homePos = request.FromPos;

        var scope = context.GetHttpContext().RequestServices.CreateScope();

        GetPlace getPlace = scope.ServiceProvider.GetRequiredService<GetPlace>();
        var response = await getPlace.GetTextPos(prompt, homePos);

        return new PredictPositionsFromTextResponse
        {
            Place = { response }
        };
    }

    [Authorize]
    public override async Task<PredictTimeTableResponse> PredictTimeTable(PredictTimeTableRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (request.Uid.Value != authUser.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var eventMaterial = request.EventMaterial;
        var isStart = request.IsGoing;

        var scope = context.GetHttpContext().RequestServices.CreateScope();

        GetTimeTable getTimeTable = scope.ServiceProvider.GetRequiredService<GetTimeTable>();
        var response = await getTimeTable.GetTimeTableList(eventMaterial, isStart);

        return new PredictTimeTableResponse
        {
            TimeTable = { response }
        };
    }
}
