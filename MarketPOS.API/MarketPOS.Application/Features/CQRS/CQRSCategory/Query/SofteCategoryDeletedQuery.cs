using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
public record SofteCategoryDeletedQuery : IRequest<ResultDto<SofteDeleteDto>>
{
    public Guid Id { get; set; }

    public SofteCategoryDeletedQuery(Guid id)
    {
        Id = id;
    }
}
