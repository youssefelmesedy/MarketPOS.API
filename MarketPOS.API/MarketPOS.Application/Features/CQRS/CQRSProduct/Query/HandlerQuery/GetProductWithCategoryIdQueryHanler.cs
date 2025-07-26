using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Specifications.Products;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;

public class GetProductWithCategoryIdQueryHanler : BaseHandler<GetProductWithCategoryIdQueryHanler>, IRequestHandler<GetProductWithCategoryIdQuery, ResultDto<PagedResultDto<ProductDetailsDto>>>
{
    public GetProductWithCategoryIdQueryHanler(
        IServiceFactory services,
        IResultFactory<GetProductWithCategoryIdQueryHanler> resultFactory,
        IMapper? mapper = null,
        ILogger<GetProductWithCategoryIdQueryHanler>? logger = null,
        IStringLocalizer<GetProductWithCategoryIdQueryHanler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!) 
        : base(services, resultFactory, mapper, logger, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<PagedResultDto<ProductDetailsDto>>> Handle(GetProductWithCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var service = _servicesFactory.GetService<IProductService>();

        var includes = new List<ProductInclude>
        {
            ProductInclude.Category,
            ProductInclude.Product_Price,
            ProductInclude.Product_UnitProfile,
            ProductInclude.Product_InventorieAndWareHouse
        };

        var spec = new GetProductWithIncludesSpecification(request.CategoryId, includes, request.IncludeSofteDelete, request.PageSize, request.PageIndex);

        var products = await service.GetProductbyCategoryIdSpce(spec);

        if (products is null || !products.Any())
        {
            return _resultFactory.Fail<PagedResultDto<ProductDetailsDto>>($"Not Found Data With Category Id: {request.CategoryId}");
        }

        var mapping = _mapper?.Map<IEnumerable<ProductDetailsDto>>(products);
        if (mapping is null)
            return _resultFactory.Fail<PagedResultDto<ProductDetailsDto>>("Mapping Failed");
        
        var pagenition = new PagedResultDto<ProductDetailsDto>(mapping, products.Count(), request.PageIndex, request.PageSize);

        if(pagenition is null || pagenition.TotalCount == 0)
            return _resultFactory.Fail<PagedResultDto<ProductDetailsDto>>("Mapping Failed");

        var localized = _localizationPostProcessor?.Apply(pagenition!);
        if (localized is null)
            return _resultFactory.Fail<PagedResultDto<ProductDetailsDto>>("Localization Failed");

        return _resultFactory.Success(localized, "Get Product With Category Id Success");
    }
}
