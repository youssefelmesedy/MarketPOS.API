using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Localization;


namespace MarketPOS.API.Extensions.ExtensionLocalizetion;

public static class LocalizationExtensions
{
    public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddSingleton<JsonLocalizationCache>();
        services.AddSingleton<IStringLocalizerFactory>(provider =>
        {
            var cache = provider.GetRequiredService<JsonLocalizationCache>();
            return new CustomJsonStringLocalizerFactory(cache, "Resources");
        });
        services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        services.AddScoped<ILocalizationPostProcessor, LocalizationPostProcessor>();

        services.AddMvc()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(JsonStringLocalizer));
            });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar-EG"),
            };

            options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
            options.SupportedCultures = supportedCultures;
        });

        return services;
    }

    public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "ar-EG", "en-US" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        var locCacheService = app.ApplicationServices.GetRequiredService<JsonLocalizationCache>();
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        lifetime.ApplicationStopping.Register(() =>
        {
            locCacheService.Clear();
            Console.WriteLine("✅ Localization cache cleared on shutdown.");
        });

        return app;
    }
}