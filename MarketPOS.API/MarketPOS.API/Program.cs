using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Infrastructure.ImplmentationCacheing;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
           options.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
       });

// ✅ Add services to the container
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddDesignPatternServices();

// ✅ Cache Provider (Memory or Redis)
var cacheProvider = builder.Configuration["CacheSettings:Provider"];

if (cacheProvider?.Equals("Redis", StringComparison.OrdinalIgnoreCase) == true)
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["CacheSettings:RedisConnection"];
        options.InstanceName = "MarketPOS_";
    });
    builder.Services.AddScoped<IGenericCache, RedisCacheService>();
}
else
{
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<IGenericCache, MemoryCacheService>();
}

// ✅ Localization
builder.Services.AddLocalization();
builder.Services.AddSingleton<JsonLocalizationCache>();
builder.Services.AddSingleton<IStringLocalizerFactory>(provider =>
{
    var cache = provider.GetRequiredService<JsonLocalizationCache>();
    return new CustomJsonStringLocalizerFactory(cache, "Resources");
});
builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
builder.Services.AddScoped<ILocalizationPostProcessor, LocalizationPostProcessor>();

// ✅ FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// ✅ MVC + DataAnnotations Localization
builder.Services.AddMvc()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(JsonStringLocalizer));
    });

// ✅ Supported cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar-EG"),
        new CultureInfo("en-US"),
    };

    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
});

// ✅ Swagger
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "MarketPOS API";
    config.OperationProcessors.Add(new AcceptLanguageHeaderProcessor());
});

// ✅ Build app
var app = builder.Build();

// ✅ Swagger middleware
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseOpenApi();
    app.UseSwaggerUi();

    // Redirect root "/" to swagger
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger/index.html");
        return Task.CompletedTask;
    });
}


// ✅ Use routing
app.UseHttpsRedirection();

// ✅ Localization middleware
var supportedCultures = new[] { "ar-EG", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// ✅ Clear localization cache on shutdown
var locCacheService = app.Services.GetRequiredService<JsonLocalizationCache>();
app.Lifetime.ApplicationStopping.Register(() =>
{
    locCacheService.Clear();
    Console.WriteLine("✅ Localization cache cleared on shutdown.");
});

app.UseAuthorization();

// ✅ Middleware مخصص
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ResponseMiddleware>();

// ✅ Map endpoints
app.MapControllers();

app.Run();
