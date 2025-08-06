using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query.QueryHandler;
public class RestorWareHouseQueryHandler : BaseHandler<RestorWareHouseQueryHandler>,
    IRequestHandler<RestorWareHouseQuery, ResultDto<SofteDeleteAndRestorDto>>
{
    public RestorWareHouseQueryHandler(
        IServiceFactory services,
        IResultFactory<RestorWareHouseQueryHandler> resultFactory,
        IMapper? mapper = null, 
        IStringLocalizer<RestorWareHouseQueryHandler>? localizer = null, 
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<SofteDeleteAndRestorDto>> Handle(RestorWareHouseQuery request, CancellationToken cancellationToken)
    {
        var wareHouseService = _servicesFactory.GetService<IWareHouseService>();

        var wareHouse = await wareHouseService.GetByIdAsync(request.Id, includeSoftDeleted: true);
        if (wareHouse is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("GetByIdFailed");

        if(!wareHouse.IsDeleted)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("RestoreFailed");

        var result = await wareHouseService.RestoreAsync(wareHouse);

        var mappedResult = _mapper?.Map<SofteDeleteAndRestorDto>(result);
        if (mappedResult is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("MappingFailed");

        var localizedResult = _localizationPostProcessor.Apply(mappedResult);

        return _resultFactory.Success(localizedResult, "Restored");
    }
}
