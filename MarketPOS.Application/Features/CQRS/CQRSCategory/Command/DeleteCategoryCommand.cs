namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command;

public record DeleteCategoryCommand : IRequest<ResultDto<Guid>>
{
    public Guid Id { get; set; }

    public DeleteCategoryCommand(Guid id)
    {
        Id = id;
    }
}
