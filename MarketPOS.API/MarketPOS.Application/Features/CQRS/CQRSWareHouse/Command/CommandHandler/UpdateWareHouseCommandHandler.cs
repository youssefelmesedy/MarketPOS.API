using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.WareHouseDTO;
namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Command.CommandHandler;
public class UpdateWareHouseCommandHandler : BaseHandler<UpdateWareHouseCommandHandler>,
    IRequestHandler<UpdateWareHouseCommand, ResultDto<Guid>>
{
    public UpdateWareHouseCommandHandler(
        IServiceFactory services,
        IResultFactory<UpdateWareHouseCommandHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<UpdateWareHouseCommandHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<Guid>> Handle(UpdateWareHouseCommand request, CancellationToken cancellationToken)
    {
        var wareHouseService = _servicesFactory.GetService<IWareHouseService>();

        var normalizedName = request.Dto.Name.Trim().ToLower();

        if (await wareHouseService.AnyAsync(p =>
                                          (p.Name.Trim().ToLower() == normalizedName) &&
                                                    p.Id != request.Dto.Id))
            return _resultFactory.Fail<Guid>($"DuplicateWareHouseName");

        var wareHouse = await wareHouseService.GetByIdAsync(request.Dto.Id, true);
        if (wareHouse is null)
            return _resultFactory.Fail<Guid>("GetByIdFailed");

        UpdateWareHouse(wareHouse, request.Dto);

        await wareHouseService.UpdateAsync(wareHouse);

        return _resultFactory.Success(wareHouse.Id, "Updated");

    }

    private void UpdateWareHouse(Warehouse wareHouse, WareHouseUpdateDto dto)
    {
        if (dto.Name is not null)
            wareHouse.Name = dto.Name;

        if (wareHouse.ContactInfo is not null && dto.ContactInfoDto is not null)
        {
            if (dto.ContactInfoDto.Phone is not null)
                wareHouse.ContactInfo.Phone = dto.ContactInfoDto.Phone;

            if (dto.ContactInfoDto.Email is not null)
                wareHouse.ContactInfo.Email = dto.ContactInfoDto.Email;

            if (wareHouse.ContactInfo.Address is not null && dto.ContactInfoDto.Address is not null)
            {
                if (dto.ContactInfoDto.Address.Country is not null)
                    wareHouse.ContactInfo.Address.Country = dto.ContactInfoDto.Address.Country;

                if (dto.ContactInfoDto.Address.City is not null)
                    wareHouse.ContactInfo.Address.City = dto.ContactInfoDto.Address.City;

                if (dto.ContactInfoDto.Address.Street is not null)
                    wareHouse.ContactInfo.Address.Street = dto.ContactInfoDto.Address.Street;
            }
        }
    }
}
