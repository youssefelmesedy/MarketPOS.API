namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class GetAllProductsHandler : BaseHandler<GetAllProductsHandler>, IRequestHandler<GetAllProductsQuery, ResultDto<IEnumerable<SomeFeaturesProductDto>>>
{
    public GetAllProductsHandler(
        IServiceFactory factoryService, 
        IResultFactory<GetAllProductsHandler> resultFactory, 
        IMapper mapper,
        IStringLocalizer<GetAllProductsHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor)
        : base(factoryService, resultFactory,mapper, null, localizer,localizationPostProcessor)
    {}

    public async Task<ResultDto<IEnumerable<SomeFeaturesProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var productService = _servicesFactory.GetService<IProductService>();

        var includes = ProductIncludeHelper.GetIncludeExpressions(
            new List<ProductInclude>
            {
                ProductInclude.Category,
                ProductInclude.Product_Inventorie,
                ProductInclude.Product_Price
            });

        var data = await productService.GetAllAsync(false, includes, request.SofteDelete);
        if (data is null || !data.Any())
            return _resultFactory.Fail<IEnumerable<SomeFeaturesProductDto>>("GetAllFailed");

        var mapped = _mapper?.Map<IEnumerable<SomeFeaturesProductDto>>(data);
        if(mapped is null)
            return _resultFactory.Fail<IEnumerable<SomeFeaturesProductDto>>("MappingError");

        var localized = _localizationPostProcessor.Apply(mapped);

        return _resultFactory.Success(localized!, "Success");
    }
}
