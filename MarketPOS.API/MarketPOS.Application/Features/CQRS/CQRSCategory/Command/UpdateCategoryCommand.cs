namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command;
public record UpdateCategoryCommand : IRequest<ResultDto<Guid>>
{
    public bool SofteDelete { get; set; }
    public CategoryUpdateDto dto { get; set; }

    public UpdateCategoryCommand(CategoryUpdateDto dto, bool softeDelete)
    {
        this.dto = dto;
        SofteDelete = softeDelete;
    }
}
