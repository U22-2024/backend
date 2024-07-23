using GrpcService;
using Microsoft.EntityFrameworkCore;

namespace DbInitializer;

public static class UserTestData
{
    public static async Task AddData(AppDbContext dbCtx)
    {
        if (await dbCtx.Users.AnyAsync()) return;
    }
}