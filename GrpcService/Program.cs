using System.Security.Claims;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TodoService = GrpcService.Services.TodoService;

var builder = WebApplication.CreateBuilder(args);

var firebaseOpt = new AppOptions
{
    Credential = GoogleCredential.GetApplicationDefault(),
    ProjectId = "u22-2024"
};
FirebaseApp.Create(firebaseOpt);

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
        opt.Authority = $"https://securetoken.google.com/{firebaseOpt.ProjectId}";
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseOpt.ProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseOpt.ProjectId,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<TodoService>();

if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

app.Run();