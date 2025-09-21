using MarketPOS.API.Extensions.ExtensionCacheing;
using MarketPOS.API.Extensions.ExtensionLocalizetion;
using MarketPOS.API.Extensions.ExtensionMiddlewar;
using MarketPOS.API.Extensions.ExtensionSwgger;
using MarketPOS.API.Extensions.ExtensionValidatuion;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddDesignPatternServices()
    .AddCustomCaching(builder.Configuration)
    .AddCustomLocalization()
    .AddCustomValidation()
    .AddCustomSwagger();

var app = builder.Build();

app.UseCustomSwagger();
app.UseCustomLocalization();
app.UseCustomMiddlewares();
app.MapControllers();

app.Run();
