using Event.V1;
using GrpcService;
using GrpcService.API;
using GrpcService.Extensions;
using GrpcService.Models.Greet;
using GrpcService.Services;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;
using DateTime = Event.V1.DateTime;
using EventService = GrpcService.Services.EventService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.SetupApp();

builder.Services.AddScoped<PredictEventMaterial>();
builder.Services.AddScoped<GetPlace>();
builder.Services.AddScoped<GetTimeTable>();

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
app.MapGrpcService<GreetService>();
app.MapGrpcService<EventService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();
using (var scope = app.Services.CreateScope())
await using (var dbCtx = scope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    var strategy = dbCtx.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(() => dbCtx.Database.EnsureCreatedAsync());

    // 一言メッセージを読み込んでデータベースに保存する
    var wb = new Workbook();
    wb.LoadFromFile("../mother_hitokoto.xlsx");

    var worksheet = wb.Worksheets[0];

    for (var row = 1; row <= worksheet.LastRow; row++)
    {
        var range = worksheet.Range[row, 1];
        var cellValue = range.Text == null ? string.Empty : range.Text;
        var greet = new GreetModel { Id = row, Message = cellValue };

        dbCtx.Greets.Add(greet);
    }

    // リソースを解放する
    wb.Dispose();

    // DBへ書き込み
    await dbCtx.SaveChangesAsync();
}

app.Run();
