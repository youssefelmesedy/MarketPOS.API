using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query;
public class GetAllActiveIngredinentQuery : IRequest<ResultDto<IEnumerable<ActiveIngredinentsDetalisDTO>>>
{
    public bool SoftDelete { get; set; }

    public GetAllActiveIngredinentQuery(bool softDelete)
    {
        SoftDelete = softDelete;
    }
}
