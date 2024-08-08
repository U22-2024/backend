using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models.Remind;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Remind.V1;

namespace GrpcService.Services;

public class RemindGroupService(AppDbContext dbContext, ILogger<RemindGroupService> logger)
    : Remind.V1.RemindGroupService.RemindGroupServiceBase
{
    [Authorize]
    public override async Task<CreateRemindGroupResponse> CreateRemindGroup(CreateRemindGroupRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (request.Uid != authUser.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var remindGroup = new RemindGroupModel
        {
            Title = request.Title,
            Description = request.Description,
            Uid = request.Uid
        };
        await dbContext.RemindGroups.AddAsync(remindGroup);
        await dbContext.SaveChangesAsync();

        return new CreateRemindGroupResponse
        {
            RemindGroup = new RemindGroup
            {
                Id = remindGroup.Id.ToString(),
                Title = remindGroup.Title,
                Description = remindGroup.Description,
                Uid = remindGroup.Uid
            }
        };
    }

    [Authorize]
    public override async Task<GetRemindGroupResponse> GetRemindGroup(GetRemindGroupRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindGroup = await dbContext.RemindGroups.FindAsync(Guid.Parse(request.Id));
        if (remindGroup == null)
            throw new RpcException(new Status(StatusCode.NotFound, "RemindGroup not found"));
        if (authUser.Uid != remindGroup.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        return new GetRemindGroupResponse
        {
            RemindGroup = new RemindGroup
            {
                Id = remindGroup.Id.ToString(),
                Title = remindGroup.Title,
                Description = remindGroup.Description,
                Uid = remindGroup.Uid
            }
        };
    }

    [Authorize]
    public override async Task<GetRemindGroupsResponse> GetRemindGroups(GetRemindGroupsRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser(logger);
        var remindGroups = await dbContext.RemindGroups.Where(x => x.Uid == authUser.Uid).ToListAsync();

        return new GetRemindGroupsResponse
        {
            RemindGroups =
            {
                remindGroups.Select(x => new RemindGroup
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Uid = x.Uid
                })
            }
        };
    }

    [Authorize]
    public override async Task<UpdateRemindGroupResponse> UpdateRemindGroup(UpdateRemindGroupRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindGroup = await dbContext.RemindGroups.FindAsync(Guid.Parse(request.Id));
        if (remindGroup == null)
            throw new RpcException(new Status(StatusCode.NotFound, "RemindGroup not found"));
        if (authUser.Uid != remindGroup.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        remindGroup.Title = request.Title;
        remindGroup.Description = request.Description;
        await dbContext.SaveChangesAsync();

        return new UpdateRemindGroupResponse
        {
            RemindGroup = new RemindGroup
            {
                Id = remindGroup.Id.ToString(),
                Title = remindGroup.Title,
                Description = remindGroup.Description,
                Uid = remindGroup.Uid
            }
        };
    }

    [Authorize]
    public override async Task<DeleteRemindGroupResponse> DeleteRemindGroup(DeleteRemindGroupRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindGroup = await dbContext.RemindGroups.FindAsync(Guid.Parse(request.Id));
        if (remindGroup == null)
            throw new RpcException(new Status(StatusCode.NotFound, "RemindGroup not found"));
        if (authUser.Uid != remindGroup.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        // リマインドグループに紐づくリマインドも削除
        dbContext.Reminds.RemoveRange(remindGroup.Reminds);
        dbContext.RemindGroups.Remove(remindGroup);
        await dbContext.SaveChangesAsync();

        return new DeleteRemindGroupResponse();
    }
}
