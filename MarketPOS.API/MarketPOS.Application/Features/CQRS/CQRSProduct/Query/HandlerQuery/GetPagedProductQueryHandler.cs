using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class GetPagedProductQueryHandler : BaseHandler<GetPagedProductQueryHandler>,
    IRequestHandler<GetPagedProductQuery, ResultDto<PagedResultDto<ProductDetailsDto>>>
{

    public GetPagedProductQueryHandler(IServiceFactory serviceFactory,
        IResultFactory<GetPagedProductQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetPagedProductQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : base(serviceFactory, resultFactory, mapper, localizer: localizer, localizationPostProcessor: localizationPostProcessor)
    {
    }

    public async Task<ResultDto<PagedResultDto<ProductDetailsDto>>> Handle(GetPagedProductQuery request, CancellationToken cancellationToken)
    {
        var includes = ProductIncludeHelper.GetIncludeExpressions(request.Includes);

        var productService = _servicesFactory.GetService<IProductService>();

        var result = await productService.GetPagedProjectedAsync<ProductDetailsDto>(_mapper!,
            request.PageIndex,
            request.PageSize,
            ordering: p => p.OrderBy(p => p.Name),
            includeExpressions: includes,
            includeSoftDeleted: request.SofteDelete
        );

        if (result.Data is null || result.TotalCount == 0)
            return _resultFactory.Fail<PagedResultDto<ProductDetailsDto>>("GetPagedFailed");


        var localizedDtoList = _localizationPostProcessor.Apply<ProductDetailsDto>(result.Data);

        var pagedResult = new PagedResultDto<ProductDetailsDto>(localizedDtoList, result.TotalCount, request.PageIndex, request.PageSize);

        return _resultFactory.Success(pagedResult, "Success");
    }

}
