using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TodoService = GrpcService.Services.TodoService;

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
        policy.RequireClaim(ClaimTypes.Name);
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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<TodoService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

app.Run();