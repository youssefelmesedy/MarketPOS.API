using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query;
public class GetIngredinentByNameQuery : IRequest<ResultDto<ActiveIngredinentsDetalisDTO>>
{
    public string Name { get; set; } = string.Empty;
    public bool SofteDeleted { get; set; }
    public GetIngredinentByNameQuery(string name, bool softeDeleted)
    {
        Name = name;
        SofteDeleted = softeDeleted;
    }
}
