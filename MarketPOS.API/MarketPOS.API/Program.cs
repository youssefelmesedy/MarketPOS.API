using Hangfire;
using MarketPOS.API.Bootstrapper.Extensions.ExtensionCacheing;
using MarketPOS.API.Extensions;
using MarketPOS.API.Extensions.ExtensionLocalizetion;
using MarketPOS.API.Extensions.ExtensionMiddlewar;
using MarketPOS.API.Extensions.ExtensionSwgger;
using MarketPOS.API.Extensions.ExtensionValidatuion;
using MarketPOS.API.Extensions.SetteingEmailService;
using MarketPOS.Application;
using MarketPOS.Shared.RateLimitedSettings;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.JWT.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.RateLimitRoles.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Email.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<RateLimitingOptions>(builder.Configuration.GetSection("RateLimitingOptions"));

builder.Services
    .AddApplicationServices()
    .AddEmailService(builder.Configuration, builder.Environment)
    .AddInfrastructureServices(builder.Configuration, builder.Environment) 
    .AddDesignPatternServices()
    .AddCustomCaching(builder.Configuration)
    .AddCustomLocalization()
    .AddCustomValidation()
    .AddCustomSwagger()
    .AddCustomAuthenticationAndAuthorization(builder.Configuration); 

var app = builder.Build();

// Middlewares
app.UseCustomSwagger();
app.UseCustomLocalization();
app.UseCustomMiddlewares();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "MarketPOS Background Jobs",
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapControllers();

app.Run();
