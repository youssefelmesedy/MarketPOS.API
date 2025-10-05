using MarketPOS.Shared.JWTSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MarketPOS.API.Extensions;
public static class ExtensionAuthenticationAndAuthorizationPipeline
{
    public static IServiceCollection AddCustomAuthenticationAndAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JWT>(configuration.GetSection("JWT"));

        var jwtSection = configuration.GetSection("JWT");
        var jwtKey = jwtSection["Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new ArgumentNullException("Jwt:Key", "JWT Key is not configured in appsettings.JWT.json");
        }

        var key = Encoding.UTF8.GetBytes(jwtKey);


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero 
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    // عشان تمنع ASP.NET Core من إنه يكتب Default Response
                    context.HandleResponse();

                    if (!context.Response.HasStarted)
                    {
                        var problem = new ExtendedProblemDetails
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Unauthorized",
                            Detail = "You are not authorized to access this resource.",
                            ErrorCode = "AUTH_001",
                            ErrorSource = "JWT",
                            Instance = context.Request.Path,
                            Errors = new { Authorization = new[] { "Missing or invalid token" } }
                        };

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var json = JsonSerializer.Serialize(problem);
                        await context.Response.WriteAsync(json);
                    }
                },
                OnForbidden = async context =>
                {
                    if (!context.Response.HasStarted)
                    {
                        var problem = new ExtendedProblemDetails
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Forbidden",
                            Detail = "You do not have permission to access this resource.",
                            ErrorCode = "AUTH_002",
                            ErrorSource = "JWT",
                            Instance = context.Request.Path,
                            Errors = new { Authorization = new[] { "You lack required roles or policies" } }
                        };

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var json = JsonSerializer.Serialize(problem);
                        await context.Response.WriteAsync(json);
                    }
                }
            };
        });

        services.AddAuthorization();

        return services;
    }
    public static IApplicationBuilder UseCustomAuthenticationAndAuthorization(
       this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}

