using System.Security.Claims;
using GrpcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoService = GrpcService.Services.TodoService;
using UserService = GrpcService.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

const string firebaseProjectId = "u22-2024";
// var firebaseOpt = new AppOptions
// {
//     Credential = GoogleCredential.GetApplicationDefault(),
//     ProjectId = "u22-2024"
// };
// FirebaseApp.Create(firebaseOpt);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireClaim(ClaimTypes.Email);
    });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true
        };
    });
builder.Services.AddDbContextPool<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddHttpLogging(opt =>
{
    opt.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponseStatusCode;
});
builder.Logging.AddSimpleConsole(opt => { opt.IncludeScopes = true; });
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseHttpLogging();
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<UserService>();
app.MapGrpcService<TodoService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

app.Run();