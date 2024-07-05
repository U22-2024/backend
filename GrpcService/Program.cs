using TodoService = GrpcService.Services.TodoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TodoService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

app.Run();