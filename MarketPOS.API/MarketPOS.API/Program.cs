using MarketPOS.API.Bootstrapper.Extensions.ExtensionCacheing;
using MarketPOS.API.Extensions;
using MarketPOS.API.Extensions.ExtensionLocalizetion;
using MarketPOS.API.Extensions.ExtensionMiddlewar;
using MarketPOS.API.Extensions.ExtensionSwgger;
using MarketPOS.API.Extensions.ExtensionValidatuion;
using MarketPOS.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.JWT.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration) 
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

app.MapControllers();

app.Run();
