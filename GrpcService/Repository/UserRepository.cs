using Grpc.Core;
using GrpcService.Models;

namespace GrpcService.Repository;

public class UserRepository(AppDbContext dbCtx)
{
    public async Task<User> Create(User user)
    {
        if (dbCtx.Users.Any(u => u.Uid == user.Uid))
            throw new RpcException(new Status(StatusCode.AlreadyExists, "User already exists"));
        await dbCtx.Users.AddAsync(user);
        await dbCtx.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetById(string uid)
    {
        var user = await dbCtx.Users.FindAsync(uid);
        if (user is null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        return user;
    }

    public async Task<User> UpdateById(string id, Func<User, Task> op)
    {
        var user = await GetById(id);
        await op(user);
        await dbCtx.SaveChangesAsync();
        return user;
    }

    public async Task DeleteById(string uid)
    {
        var user = await dbCtx.Users.FindAsync(uid);
        if (user is null) throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        dbCtx.Users.Remove(user);
        await dbCtx.SaveChangesAsync();
    }
}