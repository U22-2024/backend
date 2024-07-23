using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models;
using GrpcService.Repository;
using Microsoft.AspNetCore.Authorization;
using Proto.User.V1;

namespace GrpcService.Services;

public class UserService(AppDbContext dbCtx, ILogger<UserService> logger, UserRepository userRepository)
    : Proto.User.V1.UserService.UserServiceBase
{
    [Authorize]
    public override async Task<UserServiceCreateResponse> Create(UserServiceCreateRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser(logger);
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await userRepository.Create(new User
        {
            Uid = request.Uid,
            Email = request.Email,
            DisplayName = string.IsNullOrEmpty(request.DisplayName) ? request.Email : request.DisplayName,
            IconUrl = request.IconUrl
        });
        await dbCtx.SaveChangesAsync();

        return new UserServiceCreateResponse
        {
            Uid = user.Uid,
            Email = user.Email,
            DisplayName = user.DisplayName,
            IconUrl = user.IconUrl
        };
    }

    [Authorize]
    public override async Task<UserServiceReadResponse> Read(UserServiceReadRequest request, ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await userRepository.GetById(request.Uid);

        return new UserServiceReadResponse
        {
            Uid = user.Uid,
            Email = user.Email,
            DisplayName = user.DisplayName,
            IconUrl = user.IconUrl
        };
    }

    [Authorize]
    public override async Task<UserServiceUpdateResponse> Update(UserServiceUpdateRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await userRepository.UpdateById(request.Uid, user =>
        {
            user.DisplayName = string.IsNullOrEmpty(request.DisplayName) ? request.Email : request.DisplayName;
            user.IconUrl = request.IconUrl;
            return Task.CompletedTask;
        });
        await dbCtx.SaveChangesAsync();

        return new UserServiceUpdateResponse
        {
            Uid = user.Uid,
            Email = user.Email,
            DisplayName = user.DisplayName,
            IconUrl = user.IconUrl
        };
    }

    [Authorize]
    public override async Task<UserServiceDeleteResponse> Delete(UserServiceDeleteRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        await userRepository.DeleteById(request.Uid);

        return new UserServiceDeleteResponse
        {
            Success = true
        };
    }
}