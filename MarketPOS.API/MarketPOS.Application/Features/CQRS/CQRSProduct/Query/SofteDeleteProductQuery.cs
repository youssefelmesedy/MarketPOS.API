using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
public record SofteDeleteProductQuery : IRequest<ResultDto<SofteDeleteAndRestorDto>>
{
    public Guid Id { get; init; } // The ID of the product to be soft-deleted
    public bool IncludeSofteDelete { get; init; }
    public SofteDeleteProductQuery(Guid id, bool includeSofteDelte)
    {
        Id = id;
        IncludeSofteDelete = includeSofteDelte;
    }
}
