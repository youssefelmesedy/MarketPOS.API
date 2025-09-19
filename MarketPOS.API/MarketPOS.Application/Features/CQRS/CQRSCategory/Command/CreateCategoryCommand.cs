namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command;
public record CreateCategoryCommand : IRequest<ResultDto<Guid>>
{
    public CategoryCreateDto Dto { get; set; }

    public CreateCategoryCommand(CategoryCreateDto dto)
    {
        Dto = dto;
    }
}
