namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;

public record GetByIdCategoryQuery : IRequest<ResultDto<CategoryDetalisDto>> 
{
    public Guid Id { get; set; }
    public bool SoftDeleted { get; set; }

    public GetByIdCategoryQuery(Guid id, bool softDeleted)
    {
        Id = id;
        SoftDeleted = softDeleted;
    }
}
