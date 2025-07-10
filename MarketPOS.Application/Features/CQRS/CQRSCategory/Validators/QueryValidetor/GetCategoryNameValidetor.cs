using MarketPOS.Application.Features.CQRS.CQRSCategory.Query;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Validators.QueryValidetor;

public class GetCategoryNameValidetor : BaseValidator<GetCategoryName>
{
    public GetCategoryNameValidetor()
    {
        RuleForNameValid(x => x.CategoryName!, "CategoryName");
    }
}
