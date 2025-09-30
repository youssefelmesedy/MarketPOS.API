using NSwag.Generation.Processors.Security;

namespace MarketPOS.API.Extensions.ExtensionSwgger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddOpenApiDocument(config =>
        {
            config.Title = "MarketPOS API";
            config.Version = "v1";
            config.Description = "This is the API for MarketPOS system.\n\n" +
                                 "✨ Developed by Youssef ElMesedy ✨\n\n";

            // Contact & License Info
            config.PostProcess = document =>
            {
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Youssef ElMesedy",
                    Email = "yousefelmesedy6@gmail.com",
                    Url = "https://yourwebsite.com"
                };

                document.Info.License = new NSwag.OpenApiLicense
                {
                    Name = "MIT License",
                    Url = "https://opensource.org/licenses/MIT"
                };
            };

            // Accept-Language Header (لو عندك كلاس مخصص)
            config.OperationProcessors.Add(new AcceptLanguageHeaderProcessor());

            config.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http, // خلي بالك هنا
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = OpenApiSecurityApiKeyLocation.Header, // مش ParameterLocation
                Name = "Authorization",
                Description = "Enter: Bearer {your token}"
            });

            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

        });

        return services;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();

            app.AddSwaggerRedirect();
        }

        return app;
    }

    public static WebApplication AddSwaggerRedirect(this WebApplication app)
    {
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        return app;
    }
}
