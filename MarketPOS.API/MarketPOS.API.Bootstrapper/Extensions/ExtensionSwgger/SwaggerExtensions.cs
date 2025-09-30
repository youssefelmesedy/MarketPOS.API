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
                                 "✨ Developed by Y𝒐𝐔𝓢𝓢e𝓕 E𝓵𝓜𝐄𝓢𝐞𝐃𝓨 ✨\n\n" +
                                 "🔗 Website: https://yourwebsite.com\n" +
                                 "📧 Email: https://yousefelmesedy6@gmail.com\n\n" +
                                 "License: MIT License";

            config.OperationProcessors.Add(new AcceptLanguageHeaderProcessor());
            config.DocumentProcessors.Add(new ContactDocumentProcessor());
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
            context.Response.Redirect("/swagger/index.html");
            return Task.CompletedTask;
        });

        return app;
    }
}

