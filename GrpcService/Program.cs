using Event.V1;
using GrpcService;
using GrpcService.API;
using GrpcService.Extensions;
using GrpcService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.SetupApp();

builder.Services.AddScoped<PredictEvent>();
builder.Services.AddScoped<GetPlace>();

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
    // var ai = scope.ServiceProvider.GetRequiredService<GetPlace>();
    // var pos = new Pos();
    // pos.Lat = 34.23404579573394;
    // pos.Lon = 133.6358061308664;
    // ai.GetTextPos("イオン", pos);

    // var ai = scope.ServiceProvider.GetRequiredService<PredictEvent>();
    // var eventMaterial = new EventMaterial();
    // EventMaterial after = await ai.PredictEventMaterial("明日の9時に友達とバイクでイオンに行く。今日は2024/08/24", eventMaterial);
}

app.Run();
