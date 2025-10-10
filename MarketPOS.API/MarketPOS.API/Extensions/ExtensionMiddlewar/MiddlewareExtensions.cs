using Hangfire;
using MarketPOS.API.Extensions.ExtensionsProfilingMiddleware;
using MarketPOS.API.ExtensionsFiltreingAndMiddlewares.Exceptions;
using MarketPOS.Infrastructure.TrackingServicesMiddleware;
namespace MarketPOS.API.Extensions.ExtensionMiddlewar;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        // 1️⃣ إمساك الأخطاء أول حاجة
        app.UseMiddleware<ExceptionMiddleware>();

        // 2️⃣ تجهيز الأساسيات
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        
        // 3️⃣ تسجيل الـ User (Authentication/Authorization)
        app.UseCustomAuthenticationAndAuthorization();

        // 4️⃣ بعد ما الـ User اتعرف → طبق Rate Limiting
        app.UseMiddleware<RateLimitingMiddleware>();

        //Profilinf Middleware
        app.UseMiddleware<ProfilingMiddleware>();

        // 5️⃣ أي Response Modifications
        app.UseMiddleware<ResponseMiddleware>();

        // 6️⃣ Service Tracking
        app.UseServiceTracking();

        return app;
    }
}

