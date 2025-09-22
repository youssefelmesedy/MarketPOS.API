namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command;
public record CreateCategoryCommand : IRequest<ResultDto<Guid>>
{
    public CreateCategoryDto Dto { get; set; }

    public CreateCategoryCommand(CreateCategoryDto dto)
    {
        Dto = dto;
    }
}
