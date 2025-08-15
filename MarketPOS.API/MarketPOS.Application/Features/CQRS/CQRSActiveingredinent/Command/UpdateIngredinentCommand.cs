using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command;
public class UpdateIngredinentCommand : IRequest<ResultDto<Guid>>
{
    public Guid Id { get; set; }
    public CommandActiveIngredinentsDTO Dto { get; set; }
    public UpdateIngredinentCommand(Guid id, CommandActiveIngredinentsDTO dto)
    {
        Id = id;
        Dto = dto;
    }
}
