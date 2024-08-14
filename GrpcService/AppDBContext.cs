using GrpcService.Models.Remind;
using GrpcService.Models.RemindTemplate;
using Microsoft.EntityFrameworkCore;

namespace GrpcService;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
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
            modelBuilder.Entity(entityType.Name).Property("UpdatedAt").ValueGeneratedOnUpdate();
        }
    }
}
