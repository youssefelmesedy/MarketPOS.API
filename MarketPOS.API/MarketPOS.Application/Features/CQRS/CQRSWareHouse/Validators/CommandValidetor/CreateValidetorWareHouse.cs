namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Validators.CommandValidetor;

public class CreateValidetorWareHouse : BaseValidator<WareHouseCreateDto>
{
    public CreateValidetorWareHouse()
    {
        RuleForName(w => w.Name, 100);

    }
}
