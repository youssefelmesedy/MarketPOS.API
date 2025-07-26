namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
public record SofteCategoryDeletedQuery : IRequest<ResultDto<Guid>>
{
    public Guid Id { get; set; }

    public SofteCategoryDeletedQuery(Guid id)
    {
        Id = id;
    }
}
