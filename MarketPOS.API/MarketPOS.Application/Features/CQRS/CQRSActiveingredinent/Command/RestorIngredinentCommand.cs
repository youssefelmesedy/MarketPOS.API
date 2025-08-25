using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
public class RestorIngredinentCommand : IRequest<ResultDto<RestorDto>>
{
    public Guid Id { get; set; }
    public RestorIngredinentCommand(Guid id)
    {
        Id = id;
    }
}
