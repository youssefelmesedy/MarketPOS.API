
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using System.Diagnostics;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class GetAllProductsHandler : BaseHandler<GetAllProductsHandler>, IRequestHandler<GetAllProductsQuery, ResultDto<List<SomeFeaturesProductDto>>>
{

    public GetAllProductsHandler(
        IServiceFactory factoryService,
        IResultFactory<GetAllProductsHandler> resultFactory,
        IMapper mapper,
        ILogger<GetAllProductsHandler> logger,
        IStringLocalizer<GetAllProductsHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor)
        : base(factoryService, resultFactory, mapper, logger, localizer, localizationPostProcessor)
    { }


    public async Task<ResultDto<List<SomeFeaturesProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger?.LogInformation("Handler started for GetAllProductsQuery");

        var productService = _servicesFactory.GetService<IProductService>();
        var includes = ProductIncludeHelper.GetIncludeExpressions(
            new List<ProductInclude>
            {
                ProductInclude.Category,
                ProductInclude.Product_InventorieAndWareHouse,
                ProductInclude.Product_Price,
                ProductInclude.Ingredinent,
            });

        var data = await productService.GetAllAsync<SomeFeaturesProductDto>(
            _mapper!,
            predicate: null,
            tracking: false,
            includeExpressions: includes,
            includeSoftDeleted: request.SofteDelete, ordering: p => p.OrderBy(p => p.Name));
        stopwatch.Stop();
        _logger?.LogInformation("Handler finished in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

        if (data is null || !data.Any())
            return _resultFactory.Fail<List<SomeFeaturesProductDto>>("NotFound");

        var localized = _localizationPostProcessor.Apply<SomeFeaturesProductDto>(data);

        return _resultFactory.Success(localized.ToList(), "Success");
    }
}

