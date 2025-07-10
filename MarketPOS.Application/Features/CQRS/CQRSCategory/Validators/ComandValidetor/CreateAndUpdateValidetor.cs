namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Validators.ComandValidetor;

public class CreateAndUpdateValidetor : BaseValidator<CategoryCreateDto>
{
    public CreateAndUpdateValidetor()
    {
        RuleForName(x => x.Name!, 100);
        RuleForName(x => x.Description!, 100);
    }
}
