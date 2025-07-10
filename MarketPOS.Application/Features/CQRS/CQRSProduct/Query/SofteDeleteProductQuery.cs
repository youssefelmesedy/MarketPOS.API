namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
public record SofteDeleteProductQuery : IRequest<ResultDto<ProductDetailsDto>>
{
    public Guid Id { get; init; } // The ID of the product to be soft-deleted
    public bool IncludeSofteDelete { get; init; }
    public SofteDeleteProductQuery(Guid id, bool includeSofteDelte)
    {
        Id = id;
        IncludeSofteDelete = includeSofteDelte;
    }
}
