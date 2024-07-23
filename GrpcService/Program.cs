using GrpcService.Extensions;
using TodoService = GrpcService.Services.TodoService;
using UserService = GrpcService.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.SetupApp();

var app = builder.Build();

app.UseHttpLogging();
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<UserService>();
app.MapGrpcService<TodoService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

app.Run();