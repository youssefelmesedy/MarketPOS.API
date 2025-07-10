namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command;

public class DeleteProductCommand : IRequest<ResultDto<Guid>>
{
    public Guid Id { get;}
    public DeleteProductCommand(Guid id) => Id = id;
}
