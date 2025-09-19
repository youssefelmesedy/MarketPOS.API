namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Validators.QueryValidetor;
public class GetByIdCategoryQueryValidator : BaseValidator<GetByIdCategoryQuery>
{
    public GetByIdCategoryQueryValidator()
    {
        RuleForId(x => x.Id, "ID");
    }
}
