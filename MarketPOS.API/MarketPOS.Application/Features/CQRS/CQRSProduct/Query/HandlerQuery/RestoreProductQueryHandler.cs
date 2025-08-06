using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class RestoreProductQueryHandler : BaseHandler<RestoreProductQueryHandler>,
    IRequestHandler<RestoreProductQuery, ResultDto<SofteDeleteAndRestorDto>>
{
    public RestoreProductQueryHandler(
        IServiceFactory services,
        IResultFactory<RestoreProductQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<RestoreProductQueryHandler> localizer)
        : base(services, resultFactory, mapper, null, localizer)
    {
    }

    public async Task<ResultDto<SofteDeleteAndRestorDto>> Handle(RestoreProductQuery request, CancellationToken cancellationToken)
    {
        _logger?.LogInformation(_localizer?["Starting Restore"]!);

        var productService = _servicesFactory.GetService<IProductService>();

        var product = await productService.GetByIdAsync(request.Id, includeSoftDeleted: true);
        if (product is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("GetByIdFailed");
        if (!product.IsDeleted)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("RestoreFailed");


        var result = await productService.RestoreAsync(product);
        if (result is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("RestoreFailed");

        var mapping = _mapper?.Map<SofteDeleteAndRestorDto>(result);
        if (mapping is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("MappingFailed");

        var localizedResult = _localizationPostProcessor.Apply(mapping);

        return _resultFactory.Success(localizedResult, "Restored");
    }
}
