using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ✅ Add services to the container
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddDesignPatternServices();


// ✅ Distributed cache (memory)
builder.Services.AddDistributedMemoryCache();

// ✅ Localization
builder.Services.AddLocalization();
builder.Services.AddSingleton<JsonLocalizationCache>();
builder.Services.AddSingleton<IStringLocalizerFactory>(provider =>
{
    var cache = provider.GetRequiredService<JsonLocalizationCache>();
    return new CustomJsonStringLocalizerFactory(cache, "Resources");
});
builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

// لا تسجل IStringLocalizer مباشرة، بل استخدم AddLocalization + Factory
builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>)); // ضروري لتوليد IStringLocalizer<T> بالاعتماد على factory

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
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

// ✅ Use routing
app.UseHttpsRedirection();

var supportedCultures = new[] {"ar-EG", "en-US" };

// ✅ Localization middleware
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// ✅ Clear cache on shutdown
var cacheService = app.Services.GetRequiredService<JsonLocalizationCache>();
app.Lifetime.ApplicationStopping.Register(() =>
{
    cacheService.Clear();
    Console.WriteLine("✅ Localization cache cleared on shutdown.");
});

app.UseAuthorization();

// ✅ Middleware مخصص
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ResponseMiddleware>();

// ✅ Map endpoints
app.MapControllers();

app.Run();
