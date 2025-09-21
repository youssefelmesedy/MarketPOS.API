using MarketPOS.Infrastructure.TrackingServicesMiddleware;
namespace MarketPOS.API.Extensions.ExtensionMiddlewar;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<ResponseMiddleware>();
        app.UseServiceTracking();

        return app;
    }
}
