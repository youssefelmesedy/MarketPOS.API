using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
public class CreateActivIngredinentCommand : IRequest<ResultDto<Guid>>
{
    public ActiveIngredinentsCreateDTO DTO { get; set; }

    public CreateActivIngredinentCommand(ActiveIngredinentsCreateDTO dTO)
    {
        DTO = dTO;
    }
}
