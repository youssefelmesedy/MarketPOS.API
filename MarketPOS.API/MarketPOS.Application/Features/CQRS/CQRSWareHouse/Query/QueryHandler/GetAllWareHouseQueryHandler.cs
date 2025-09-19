using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query.QueryHandler;
public class GetAllWareHouseQueryHandler : BaseHandler<GetAllWareHouseQueryHandler>,
    IRequestHandler<GetAllWareHouseQuery, ResultDto<IEnumerable<WareHouseDetailsDto>>>
{

    public GetAllWareHouseQueryHandler(
        IServiceFactory services,
        IResultFactory<GetAllWareHouseQueryHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<GetAllWareHouseQueryHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<IEnumerable<WareHouseDetailsDto>>> Handle(GetAllWareHouseQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<IWareHouseService>();

        var wareHouses = await categoryService.GetAllAsync(includeSoftDeleted: request.SofteDelete, ordering: p => p.OrderBy(w => w.Name), applyIncludes: false);
        if (wareHouses is null)
            return _resultFactory.Fail<IEnumerable<WareHouseDetailsDto>>("GetAllFailed");

        var wareHouseDtos = _mapper?.Map<IEnumerable<WareHouseDetailsDto>>(wareHouses);
        if (wareHouseDtos is null)
            return _resultFactory.Fail<IEnumerable<WareHouseDetailsDto>>("MappingFailed");

        var localizationResult = _localizationPostProcessor.Apply(wareHouseDtos);

        return _resultFactory.Success(localizationResult, "Success");
    }
}
