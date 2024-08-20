using GrpcService;
using GrpcService.Extensions;
using GrpcService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.SetupApp();

builder.Services.AddScoped<PredictRemindType>();

var app = builder.Build();

app.UseHttpLogging();
app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<RemindService>();
app.MapGrpcService<RemindGroupService>();
app.MapGrpcService<HealthCheckService>();
app.MapGrpcService<RemindTemplateService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();
using (var scope = app.Services.CreateScope())
await using (var dbCtx = scope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    var strategy = dbCtx.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(() => dbCtx.Database.EnsureCreatedAsync());
}

using (var scope = app.Services.CreateScope())
{
    var ai = scope.ServiceProvider.GetRequiredService<PredictRemindType>();
    RemindTypeResponse response = await ai.GetRemindType("トマトを買う", new string[]{"スーパー", "他買い物", "School", "Programming", "Home", "Other"});
    Console.WriteLine($"name: {response.name}, quantity: {response.quantity}");
}

app.Run();
