namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
public class GetPagedProductQuery : IRequest<ResultDto<PagedResultDto<ProductDetailsDto>>>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<ProductInclude>? Includes { get; set; }
    public bool SofteDelete { get; set; }

    public GetPagedProductQuery(int pageIndex = 1, int pageSize = 10, List<ProductInclude>? includes = null, bool softeDelete = false)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Includes = includes ?? new();
        SofteDelete = softeDelete;
    }
}
