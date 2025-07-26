namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
public class GetAllCategoryQuery : IRequest<ResultDto<List<CategoryDetalisDto>>>
{
    public bool SoftDelete { get; set; }

    public GetAllCategoryQuery(bool softDelete)
    {
        SoftDelete = softDelete;
    }
}
