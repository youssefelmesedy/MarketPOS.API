using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query.QueryHandler;
public class SofteDeleteWareHouseQueryHandler : BaseHandler<SofteDeleteWareHouseQueryHandler>,
    IRequestHandler<SofteDeleteWareHouseQuery, ResultDto<SofteDeleteDto>>
{
    public SofteDeleteWareHouseQueryHandler(
        IServiceFactory services,
        IResultFactory<SofteDeleteWareHouseQueryHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<SofteDeleteWareHouseQueryHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<SofteDeleteDto>> Handle(SofteDeleteWareHouseQuery request, CancellationToken cancellationToken)
    {
        var wareHouseService = _servicesFactory.GetService<IWareHouseService>();

        var wareHouse = await wareHouseService.GetByIdAsync(request.Id, includeSoftDeleted: true);
        if (wareHouse is null)
            return _resultFactory.Fail<SofteDeleteDto>("GetByIdFailed");

        if (wareHouse.IsDeleted)
            return _resultFactory.Fail<SofteDeleteDto>("SofteDeletedFailed");

        var result = await wareHouseService.SoftDeleteAsync(wareHouse);

        var mappedResult = _mapper?.Map<SofteDeleteDto>(result);
        if (mappedResult is null)
            return _resultFactory.Fail<SofteDeleteDto>("MappingFailed");

        var localizedResult = _localizationPostProcessor.Apply(mappedResult);

        return _resultFactory.Success(localizedResult, "SofteDeleted");
    }
}
