using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Validetor.Commandvalidetor;

public class UpdateActiveIngerdinetValidetor : BaseValidator<CommandActiveIngredinentsDTO>
{
    public UpdateActiveIngerdinetValidetor()
    {
        RuleForName(x => x.Name, 50);
    }
}
