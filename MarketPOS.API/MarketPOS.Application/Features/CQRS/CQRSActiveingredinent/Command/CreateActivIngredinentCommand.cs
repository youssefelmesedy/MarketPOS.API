using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
public class CreateActivIngredinentCommand : IRequest<ResultDto<Guid>>
{
    public CommandActiveIngredinentsDTO DTO { get; set; }

    public CreateActivIngredinentCommand(CommandActiveIngredinentsDTO dTO)
    {
        DTO = dTO;
    }
}
