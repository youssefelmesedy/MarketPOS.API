using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command.HandlerCommand;
public class DeleteProductCommandHandler : BaseHandler<DeleteProductCommandHandler>, IRequestHandler<DeleteProductCommand, ResultDto<Guid>>
{
    public DeleteProductCommandHandler
        (IServiceFactory serviceFactory,
        IResultFactory<DeleteProductCommandHandler> resultFactory,
        IStringLocalizer<DeleteProductCommandHandler> localizer) : 
        base(serviceFactory, resultFactory, localizer: localizer)
    {
    }

    public async Task<ResultDto<Guid>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productservice = _servicesFactory.GetService<IProductService>();

        var product = await productservice.GetByIdAsync(request.Id,false);
        if (product is null)
            throw new NotFoundException(nameof(Product), request.Id);

           await productservice.RemoveAsync(product);

        return _resultFactory.Success(request.Id, "Deleted");

    }
}
