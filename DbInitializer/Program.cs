using DbInitializer;
using GrpcService;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("postgres");

var host = builder.Build();

// マイグレーションを実行してデータベースを初期化
using var scope = host.Services.CreateScope();
await using (var dbCtx = scope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    await dbCtx.Database.MigrateAsync();

    // テストデータを追加
    await UserTestData.AddData(dbCtx);
}

host.Run();