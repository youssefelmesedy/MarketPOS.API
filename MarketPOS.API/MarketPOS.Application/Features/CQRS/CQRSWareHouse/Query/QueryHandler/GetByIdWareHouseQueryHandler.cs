

using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query.QueryHandler;
public class GetByIdWareHouseQueryHandler : BaseHandler<GetByIdWareHouseQueryHandler>,
    IRequestHandler<GetByIdWareHouseQuery, ResultDto<WareHouseDetailsDto>>
{
    public GetByIdWareHouseQueryHandler(
        IServiceFactory services, 
        IResultFactory<GetByIdWareHouseQueryHandler> resultFactory,
        IMapper? mapper = null, 
        IStringLocalizer<GetByIdWareHouseQueryHandler>? localizer = null, 
        ILocalizationPostProcessor localizationPostProcessor = null!) 
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<WareHouseDetailsDto>> Handle(GetByIdWareHouseQuery request, CancellationToken cancellationToken)
    {
        var wareHouseService = _servicesFactory.GetService<IWareHouseService>();

        var wareHouse = await wareHouseService.GetByIdAsync(request.Id, includeSoftDeleted: request.SofteDelete, applyIncludes: false);
        if(wareHouse is null)
            return _resultFactory.Fail<WareHouseDetailsDto>("GetByIdFailed");

        var wareHouseDto = _mapper?.Map<WareHouseDetailsDto>(wareHouse);
        if (wareHouseDto is null)
            return _resultFactory.Fail<WareHouseDetailsDto>("MappingFailed");

        var localizer = _localizationPostProcessor.Apply(wareHouseDto);

        return _resultFactory.Success(localizer, "Success");
    }
}
