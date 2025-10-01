using MarketPOS.Infrastructure.TrackingServicesMiddleware;
using Microsoft.AspNetCore.Authorization;
namespace MarketPOS.API.Extensions.ExtensionMiddlewar;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCustomAuthenticationAndAuthorization();

        app.UseMiddleware<ResponseMiddleware>();
        app.UseServiceTracking();

        return app;
    }

}
