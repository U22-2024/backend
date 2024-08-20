using GrpcService.Models.Remind;
using GrpcService.Models.RemindTemplate;
using GrpcService.Models.Shopping;
using Microsoft.EntityFrameworkCore;

namespace GrpcService;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<RemindModel> Reminds { get; init; }
    public DbSet<RemindGroupModel> RemindGroups { get; init; }

    public DbSet<RemindTemplateModel> RemindTemplates { get; init; }

    public DbSet<ShoppingItemModel> ShoppingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 全てのエンティティに対してCreatedAtとUpdatedAtがあれば自動で設定する
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.FindProperty("CreatedAt")?.SetDefaultValueSql("CURRENT_TIMESTAMP");

            var updateProp = entityType.FindProperty("UpdatedAt");
            if (updateProp == null) continue;
            updateProp.SetDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity(entityType.Name).Property("UpdatedAt").ValueGeneratedOnUpdate();
        }
    }
}
