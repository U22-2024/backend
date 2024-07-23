using System.Security.Claims;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GrpcService.Extensions;

public static class WebApplicationBuilderExt
{
    private const string FirebaseProjectId = "u22-2024";

    public static void SetupApp(this WebApplicationBuilder builder)
    {
        SetupGrpc(builder);
        SetupAuth(builder);
        SetupDb(builder);
        SetupLogging(builder);
    }

    private static void SetupFirebaseAdmin()
    {
        var firebaseOpt = new AppOptions
        {
            Credential = GoogleCredential.GetApplicationDefault(),
            ProjectId = FirebaseProjectId
        };
        FirebaseApp.Create(firebaseOpt);
    }

    private static void SetupLogging(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpLogging(opt =>
        {
            opt.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponseStatusCode;
        });
        builder.Logging.AddSimpleConsole(opt => { opt.IncludeScopes = true; });
        builder.Services.AddProblemDetails();
    }

    private static void SetupDb(WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<AppDbContext>("postgres");
    }

    private static void SetupGrpc(WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();
    }

    private static void SetupAuth(WebApplicationBuilder builder)
    {
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
                opt.Authority = $"https://securetoken.google.com/{FirebaseProjectId}";
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{FirebaseProjectId}",
                    ValidateAudience = true,
                    ValidAudience = FirebaseProjectId,
                    ValidateLifetime = true
                };
            });
    }
}