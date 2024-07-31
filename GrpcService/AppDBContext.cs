using Microsoft.EntityFrameworkCore;

namespace GrpcService;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<Models.User> Users { get; set; }
}
