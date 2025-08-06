namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Command;
public class CreateWareHouseCommand : IRequest<ResultDto<Guid>>
{
    public WareHouseCreateDto Dto { get; set; }

    public CreateWareHouseCommand(WareHouseCreateDto dto)
    {
        Dto = dto;
    }
}
