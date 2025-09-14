
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class GetAllProductsHandler : BaseHandler<GetAllProductsHandler>, IRequestHandler<GetAllProductsQuery, ResultDto<List<SomeFeaturesProductDto>>>
{

    public GetAllProductsHandler(
        IServiceFactory factoryService,
        IResultFactory<GetAllProductsHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetAllProductsHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor)
        : base(factoryService, resultFactory, mapper, null, localizer, localizationPostProcessor)
    { }


    public async Task<ResultDto<List<SomeFeaturesProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
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

        var localized = _localizationPostProcessor.Apply<SomeFeaturesProductDto>(data);

        return _resultFactory.Success(localized.ToList(), "Success");
    }
}

