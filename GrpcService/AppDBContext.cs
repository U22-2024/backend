using GrpcService.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcService;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }
}