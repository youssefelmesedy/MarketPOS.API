using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Validetor.Commandvalidetor;
public class CreateActiveIngerdinetValidetor : BaseValidator<CommandActiveIngredinentsDTO>
{
    public CreateActiveIngerdinetValidetor()
    {
        RuleForName(x => x.Name, 50);
    }
}
