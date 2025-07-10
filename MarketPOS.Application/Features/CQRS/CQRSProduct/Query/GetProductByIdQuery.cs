namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
public class GetProductByIdQuery : IRequest<ResultDto<ProductDetailsDto>>
{
    public Guid Id { get; set; }
    public bool SoftDelete { get; set; }  // Default to true to include soft-deleted products
    public GetProductByIdQuery(Guid id, bool softDelete)
    {
        Id = id;
        SoftDelete = softDelete;
    }
}
