using GrpcService.Models.Greet;
using GrpcService.Models.Remind;
using GrpcService.Models.RemindTemplate;
using Microsoft.EntityFrameworkCore;

namespace GrpcService;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<GreetModel> Greets { get; set; }

    public DbSet<RemindModel> Reminds { get; set; }
    public DbSet<RemindGroupModel> RemindGroups { get; set; }

    public DbSet<RemindTemplateModel> RemindTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 全てのエンティティに対してCreatedAtとUpdatedAtがあれば自動で設定する
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.FindProperty("UpdatedAt")?.SetDefaultValueSql("CURRENT_TIMESTAMP");
            entityType.FindProperty("CreatedAt")?.SetDefaultValueSql("CURRENT_TIMESTAMP");

            if (entityType.FindProperty("UpdatedAt") != null) {
                modelBuilder.Entity(entityType.Name).Property("UpdatedAt").ValueGeneratedOnUpdate();
            }
        }
    }
}
