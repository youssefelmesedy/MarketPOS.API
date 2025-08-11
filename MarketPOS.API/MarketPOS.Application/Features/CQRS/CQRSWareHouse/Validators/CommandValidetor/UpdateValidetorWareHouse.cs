using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Validators.CommandValidetor;

public class UpdateValidetorWareHouse : BaseValidator<WareHouseUpdateDto>
{
    public UpdateValidetorWareHouse()
    {
        RuleForName(w => w.Name, 100);

    }
}
