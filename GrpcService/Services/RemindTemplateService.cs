using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models.RemindTemplate;
using Microsoft.AspNetCore.Authorization;
using RemindTemplate.V1;

namespace GrpcService.Services;

public class RemindTemplateService(AppDbContext dbContext, ILogger<RemindTemplateService> logger)
    : RemindTemplate.V1.RemindTemplateService.RemindTemplateServiceBase
{
    [Authorize]
    public override async Task<CreateRemindTemplateResponse> CreateRemindTemplate(CreateRemindTemplateRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (request.Uid != authUser.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        var remindTemplate = new RemindTemplateModel
        {
            Title = request.Title,
            Description = request.Description,
            Uid = request.Uid
        };
        dbContext.RemindTemplates.Add(remindTemplate);
        await dbContext.SaveChangesAsync();

        return new CreateRemindTemplateResponse
        {
            RemindTemplate = new RemindTemplate.V1.RemindTemplate
            {
                Id = remindTemplate.Id.ToString(),
                Title = remindTemplate.Title,
                Description = remindTemplate.Description,
                Uid = remindTemplate.Uid
            }
        };
    }

    [Authorize]
    public override async Task<GetRemindTemplateResponse> GetRemindTemplate(GetRemindTemplateRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindTemplate = await dbContext.RemindTemplates.FindAsync(Guid.Parse(request.Id));
        if (remindTemplate == null)
            throw new RpcException(new Status(StatusCode.NotFound, "RemindTemplate not found"));
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != remindTemplate.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        return new GetRemindTemplateResponse
        {
            RemindTemplate = new RemindTemplate.V1.RemindTemplate
            {
                Id = remindTemplate.Id.ToString(),
                Title = remindTemplate.Title,
                Description = remindTemplate.Description,
                Uid = remindTemplate.Uid
            }
        };
    }

    [Authorize]
    public override Task<GetRemindTemplatesResponse> GetRemindTemplates(GetRemindTemplatesRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindTemplates = dbContext.RemindTemplates.Where(x => x.Uid == authUser.Uid).ToList();
        var response = new GetRemindTemplatesResponse();
        response.RemindTemplates.AddRange(remindTemplates.Select(x => new RemindTemplate.V1.RemindTemplate
        {
            Id = x.Id.ToString(),
            Title = x.Title,
            Description = x.Description,
            Uid = x.Uid
        }));
        return Task.FromResult(response);
    }

    [Authorize]
    public override async Task<UpdateRemindTemplateResponse> UpdateRemindTemplate(UpdateRemindTemplateRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindTemplate = await dbContext.RemindTemplates.FindAsync(Guid.Parse(request.Id));
        if (remindTemplate == null)
            throw new RpcException(new Status(StatusCode.NotFound, "RemindTemplate not found"));
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != remindTemplate.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        remindTemplate.Title = request.Title;
        remindTemplate.Description = request.Description;
        await dbContext.SaveChangesAsync();

        return new UpdateRemindTemplateResponse
        {
            RemindTemplate = new RemindTemplate.V1.RemindTemplate
            {
                Id = remindTemplate.Id.ToString(),
                Title = remindTemplate.Title,
                Description = remindTemplate.Description,
                Uid = remindTemplate.Uid
            }
        };
    }

    [Authorize]
    public override async Task<DeleteRemindTemplateResponse> DeleteRemindTemplate(DeleteRemindTemplateRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        var remindTemplate = await dbContext.RemindTemplates.FindAsync(Guid.Parse(request.Id));
        if (remindTemplate == null)
            throw new RpcException(new Status(StatusCode.NotFound, "RemindTemplate not found"));
        // リクエストユーザーがリマインドの所有者でない場合はエラーを返す
        if (authUser.Uid != remindTemplate.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied"));

        dbContext.RemindTemplates.Remove(remindTemplate);
        await dbContext.SaveChangesAsync();

        return new DeleteRemindTemplateResponse();
    }

}
