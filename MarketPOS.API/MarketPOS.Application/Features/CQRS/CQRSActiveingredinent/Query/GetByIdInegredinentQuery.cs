using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query;
public class GetByIdInegredinentQuery : IRequest<ResultDto<ActiveIngredinentsDetalisDTO>>
{
    public Guid Id { get; set; }
    public bool SofteDelete { get; set; }

    public GetByIdInegredinentQuery(Guid id, bool softeDelete)
    {
        Id = id;
        SofteDelete = softeDelete;
    }
}
