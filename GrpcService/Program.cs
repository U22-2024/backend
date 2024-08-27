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

builder.Services.AddScoped<PredictEventMaterial>();
builder.Services.AddScoped<GetPlace>();
builder.Services.AddScoped<GetRainfall>();

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
app.MapGrpcService<AdviceOutingService>();
// app.MapGrpcService<EventService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();
using (var scope = app.Services.CreateScope())
await using (var dbCtx = scope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    var strategy = dbCtx.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(() => dbCtx.Database.EnsureCreatedAsync());
}

using (var scope = app.Services.CreateScope())
{

    var ai = scope.ServiceProvider.GetRequiredService<GetPlace>();
    var pos = new Pos();
    pos.Lat = 34.23404579573394;
    pos.Lon = 133.6358061308664;
    ai.GetTextPos("イオン", pos);

    // var ai = scope.ServiceProvider.GetRequiredService<PredictEvent>();
    // var eventMaterial = new EventMaterial();
    // EventMaterial after = await ai.PredictEventMaterial("明日の9時に友達とバイクでイオンに行く。今日は2024/08/24", eventMaterial);

    var api = scope.ServiceProvider.GetRequiredService<GetRainfall>();
    var location = new Location();
    location.latitude = 34.23404579573394;
    location.longitude = 133.6358061308664;

   Console.WriteLine(await api.GetListRainfall(location));
}

app.Run();
