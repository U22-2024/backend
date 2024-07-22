using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Models;
using Microsoft.AspNetCore.Authorization;
using Proto.User.V1;

namespace GrpcService.Services;

public class UserService(AppDbContext dbCtx, ILogger<UserService> logger) : Proto.User.V1.UserService.UserServiceBase
{
    [Authorize]
    public override async Task<UserServiceCreateResponse> Create(UserServiceCreateRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("Creating user {@User}", request);

        var authUser = context.GetAuthUser(logger);
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, $"authUser.Uid({authUser.Uid}) != request.Uid({request.Uid}) (Unauthorized)"));

        var user = await dbCtx.Users.AddAsync(new User
        {
            Uid = request.Uid,
            Email = request.Email,
            DisplayName = request.DisplayName ?? request.Email,
            IconUrl = request.IconUrl
        });
        await dbCtx.SaveChangesAsync();

        return new UserServiceCreateResponse
        {
            Uid = user.Entity.Uid,
            Email = user.Entity.Email,
            DisplayName = user.Entity.DisplayName,
            IconUrl = user.Entity.IconUrl
        };
    }

    [Authorize]
    public override async Task<UserServiceReadResponse> Read(UserServiceReadRequest request, ServerCallContext context)
    {
        logger.LogTrace("Reading user {@User}", request);

        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await dbCtx.Users.FindAsync(request.Uid);
        if (user is null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

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
        logger.LogTrace("Updating user {@User}", request);

        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await dbCtx.Users.FindAsync(request.Uid);
        if (user is null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        user.DisplayName = request.DisplayName;
        user.IconUrl = request.IconUrl;
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
        logger.LogTrace("Deleting user {@User}", request);

        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await dbCtx.Users.FindAsync(request.Uid);
        if (user is null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

        dbCtx.Users.Remove(user);
        await dbCtx.SaveChangesAsync();

        return new UserServiceDeleteResponse
        {
            Success = true
        };
    }
}