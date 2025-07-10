using Market.Domain.Entitys.DomainProduct;
using Market.POS.Application.Services.Interfaces;
using MarketPOS.Application.Common.Exceptions;
using MarketPOS.Application.Common.HandlerBehaviors;
using MarketPOS.Design.FactoryResult;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

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
