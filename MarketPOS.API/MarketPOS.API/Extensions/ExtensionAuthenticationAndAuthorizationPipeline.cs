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

