using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class RestoreProductQueryHandler : BaseHandler<RestoreProductQueryHandler>,
    IRequestHandler<RestoreProductQuery, ResultDto<RestorDto>>
{
    public RestoreProductQueryHandler(
        IServiceFactory services,
        IResultFactory<RestoreProductQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<RestoreProductQueryHandler> localizer)
        : base(services, resultFactory, mapper, null, localizer)
    {
    }

    public async Task<ResultDto<RestorDto>> Handle(RestoreProductQuery request, CancellationToken cancellationToken)
    {
        var productService = _servicesFactory.GetService<IProductService>();

        var product = await productService.GetByIdAsync(request.Id, includeSoftDeleted: true);
        if (product is null)
            return _resultFactory.Fail<RestorDto>("GetByIdFailed");

        if (!product.IsDeleted)
            return _resultFactory.Fail<RestorDto>("RestoreFailed");

        var result = await productService.RestoreAsync(product);
        if (result is null)
            return _resultFactory.Fail<RestorDto>("RestoreFailed");

        var mapping = _mapper?.Map<RestorDto>(result);
        if (mapping is null)
            return _resultFactory.Fail<RestorDto>("MappingFailed");

        return _resultFactory.Success(mapping, "RestoredBy");
    }
}
