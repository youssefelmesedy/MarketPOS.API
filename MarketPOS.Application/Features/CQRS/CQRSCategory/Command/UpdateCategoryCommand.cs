namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command;

public record UpdateCategoryCommand : IRequest<ResultDto<Guid>>
{
    public CategoryUpdateDto dto { get; set; }

    public UpdateCategoryCommand(CategoryUpdateDto dto)
    {
        this.dto = dto;
    }
}
