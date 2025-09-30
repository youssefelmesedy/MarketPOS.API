using MarketPOS.Infrastructure.TrackingServicesMiddleware;
namespace MarketPOS.API.Extensions.ExtensionMiddlewar;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        // 1️⃣ Exceptions أول حاجة
        app.UseMiddleware<ExceptionMiddleware>();

        // 2️⃣ HTTPS
        app.UseHttpsRedirection();

        // 3️⃣ Static Files (زي الصور اللي هترفعها في wwwroot)
        app.UseStaticFiles();

        // 4️⃣ Routing + Authentication + Authorization
        app.UseRouting();

        // Authentication + Authorization
        app.UseCustomAuthenticationAndAuthorization();

        // 5️⃣ Middleware مخصصة بعد الـ pipeline الأساسي
        app.UseMiddleware<ResponseMiddleware>();
        app.UseServiceTracking();

        return app;
    }

}
