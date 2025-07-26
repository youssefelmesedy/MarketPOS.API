namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;

public record GetProductWithCategoryIdQuery : IRequest<ResultDto<PagedResultDto<ProductDetailsDto>>>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public Guid? CategoryId { get; init; }
    public bool IncludeSofteDelete { get; init; } = false;
    public GetProductWithCategoryIdQuery(Guid? categoryId, bool includeSofteDelete = false, int pageSize = 0, int pageIndex = 0)
    {
        CategoryId = categoryId;
        IncludeSofteDelete = includeSofteDelete;
        PageSize = pageSize;
        PageIndex = pageIndex;
    }
}
