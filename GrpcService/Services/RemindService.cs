using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models.Remind;
using Remind.V1;

namespace GrpcService.Services;

public class RemindService(AppDbContext dbContext) : Remind.V1.RemindService.RemindServiceBase
{
    public override async Task<CreateRemindResponse> CreateRemind(CreateRemindRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var remind = new RemindModel
        {
            Title = request.Title,
            Description = request.Description,
            Uid = request.Uid,
            RemindGroupId = Guid.Parse(request.GroupId)
        };

        dbContext.Reminds.Add(remind);
        await dbContext.SaveChangesAsync();

        return new CreateRemindResponse
        {
            Remind = new Remind.V1.Remind
            {
                Id = remind.Id.ToString(),
                Title = remind.Title,
                Description = remind.Description,
                Uid = remind.Uid,
                GroupId = remind.RemindGroupId.ToString()
            }
        };
    }

    public override async Task<GetRemindResponse> GetRemind(GetRemindRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remind = await dbContext.Reminds.FindAsync(Guid.Parse(request.Id));
        if (remind == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Remind not found"));
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != remind.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        return new GetRemindResponse
        {
            Remind = new Remind.V1.Remind
            {
                Id = remind.Id.ToString(),
                Title = remind.Title,
                Description = remind.Description,
                Uid = remind.Uid,
                GroupId = remind.RemindGroupId.ToString()
            }
        };
    }

    public override Task<GetRemindsResponse> GetReminds(GetRemindsRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var reminds = dbContext.Reminds.Where(elem => elem.Uid == authUser.Uid).ToList();
        var response = new GetRemindsResponse();
        response.Reminds.AddRange(reminds.Select(elem => new Remind.V1.Remind
        {
            Id = elem.Id.ToString(),
            Title = elem.Title,
            Description = elem.Description,
            Uid = elem.Uid,
            GroupId = elem.RemindGroupId.ToString()
        }));
        return Task.FromResult(response);
    }

    public override async Task<UpdateRemindResponse> UpdateRemind(UpdateRemindRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remind = await dbContext.Reminds.FindAsync(Guid.Parse(request.Id));
        if (remind == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Remind not found"));
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != remind.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        remind.Title = request.Title;
        remind.Description = request.Description;
        remind.RemindGroupId = Guid.Parse(request.GroupId);
        await dbContext.SaveChangesAsync();

        return new UpdateRemindResponse
        {
            Remind = new Remind.V1.Remind
            {
                Id = remind.Id.ToString(),
                Title = remind.Title,
                Description = remind.Description,
                Uid = remind.Uid,
                GroupId = remind.RemindGroupId.ToString()
            }
        };
    }

    public override Task<DeleteRemindResponse> DeleteRemind(DeleteRemindRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remind = dbContext.Reminds.Find(Guid.Parse(request.Id));
        if (remind == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Remind not found"));
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != remind.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        dbContext.Reminds.Remove(remind);
        dbContext.SaveChanges();

        return Task.FromResult(new DeleteRemindResponse());
    }
}
