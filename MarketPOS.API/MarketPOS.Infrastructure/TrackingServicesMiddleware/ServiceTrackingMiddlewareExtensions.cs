using Microsoft.AspNetCore.Builder;

namespace MarketPOS.Infrastructure.TrackingServicesMiddleware;

public static class ServiceTrackingMiddlewareExtensions
{
    public static IApplicationBuilder UseServiceTracking(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ServiceTrackingMiddleware>();
    }
}


