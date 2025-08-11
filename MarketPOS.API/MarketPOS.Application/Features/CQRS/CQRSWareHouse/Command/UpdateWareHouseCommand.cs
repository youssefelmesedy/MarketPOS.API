using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Command;
public class UpdateWareHouseCommand : IRequest<ResultDto<Guid>>
{
    public WareHouseUpdateDto Dto { get; set; }
    public UpdateWareHouseCommand(WareHouseUpdateDto dto)
    {
        Dto = dto;
    }
}
