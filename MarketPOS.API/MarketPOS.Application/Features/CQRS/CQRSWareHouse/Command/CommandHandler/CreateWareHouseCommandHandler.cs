using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Command.CommandHandler;
public class CreateWareHouseCommandHandler : BaseHandler<CreateWareHouseCommandHandler>,
    IRequestHandler<CreateWareHouseCommand, ResultDto<Guid>>
{
    public CreateWareHouseCommandHandler(
        IServiceFactory services,
        IResultFactory<CreateWareHouseCommandHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<CreateWareHouseCommandHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<Guid>> Handle(CreateWareHouseCommand request, CancellationToken cancellationToken)
    {
        var wareHouseService = _servicesFactory.GetService<IWareHouseService>();

        var wareHouse = _mapper?.Map<Warehouse>(request.Dto);
        if (wareHouse is null)
            return _resultFactory.Fail<Guid>("MappingFiled");

        var existWareHouseName = request.Dto.Name.Trim().ToLower();
        if (await wareHouseService.AnyAsync(i => i.Name.Trim().ToLower() == existWareHouseName, true))
            return _resultFactory.Fail<Guid>($"DuplicateWareHouseName");

        await wareHouseService.AddAsync(wareHouse);

        return _resultFactory.Success<Guid>(wareHouse.Id, "Created");
    }
}
