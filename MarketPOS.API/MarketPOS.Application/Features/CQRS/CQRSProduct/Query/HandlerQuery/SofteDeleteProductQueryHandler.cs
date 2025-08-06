using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;
public class SofteDeleteProductQueryHandler : BaseHandler<SofteDeleteProductQueryHandler>, IRequestHandler<SofteDeleteProductQuery, ResultDto<SofteDeleteAndRestorDto>>
{
    public SofteDeleteProductQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<SofteDeleteProductQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<SofteDeleteProductQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : base(serviceFactory, resultFactory, mapper, localizer: localizer, localizationPostProcessor : localizationPostProcessor)
    {
    }

    public async Task<ResultDto<SofteDeleteAndRestorDto>> Handle(SofteDeleteProductQuery request, CancellationToken cancellationToken)
    {
        var productService = _servicesFactory.GetService<IProductService>();
        var product = await productService.GetByIdAsync(request.Id, true, null, request.IncludeSofteDelete);
        if (product is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("NotFound");

        if (product.IsDeleted)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("SofteDeletedFailed");

        var result = await productService.SoftDeleteAsync(product);

        var maping = _mapper?.Map<SofteDeleteAndRestorDto>(product);
        if (maping is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("Mappingfailed");

        var localizer = _localizationPostProcessor.Apply(maping);

        return _resultFactory.Success<SofteDeleteAndRestorDto>(localizer, "SofteDeleted");
    }
}
