namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
public class RestoreCategoryQuery : IRequest<ResultDto<Guid>>
{
    public Guid Id { get; set; }

    public RestoreCategoryQuery(Guid id)
    {
        Id = id;
    }
}
