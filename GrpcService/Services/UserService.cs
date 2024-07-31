using Grpc.Core;
using GrpcService.Extensions;
using GrpcService.Repository;
using Microsoft.AspNetCore.Authorization;
using User.V1;

namespace GrpcService.Services;

public class UserService(AppDbContext dbCtx, ILogger<UserService> logger, UserRepository userRepository)
    : User.V1.UserService.UserServiceBase
{
    [Authorize]
    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser(logger);
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        var user = await userRepository.Create(new Models.User
        {
            Uid = request.Uid
        });
        await dbCtx.SaveChangesAsync();

        return new CreateUserResponse
        {
            User = new User.V1.User
            {
                Uid = user.Uid
            }
        };
    }

    [Authorize]
    public override Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.User.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        return Task.FromResult(new UpdateUserResponse
        {
            User = request.User
        });
    }

    [Authorize]
    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request,
        ServerCallContext context)
    {
        var authUser = context.GetAuthUser();
        if (authUser.Uid != request.Uid)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Unauthorized"));

        await userRepository.DeleteById(request.Uid);

        return new DeleteUserResponse
        {
            Success = true
        };
    }
}
