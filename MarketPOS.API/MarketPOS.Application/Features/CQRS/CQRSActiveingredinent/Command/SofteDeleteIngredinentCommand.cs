using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
public class SofteDeleteIngredinentCommand  : IRequest<ResultDto<SofteDeleteDto>>
{
    public Guid Id { get; set; }

    public SofteDeleteIngredinentCommand(Guid id)
    {
        Id = id;
    }
}
