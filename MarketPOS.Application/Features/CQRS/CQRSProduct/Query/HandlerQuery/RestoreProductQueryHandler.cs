using AutoMapper;
using Market.POS.Application.Services.Interfaces;
using MarketPOS.Application.Common.HandlerBehaviors;
using MarketPOS.Design.FactoryResult;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;

public class RestoreProductQueryHandler : BaseHandler<RestoreProductQueryHandler>,
    IRequestHandler<RestoreProductQuery, ResultDto<Guid>>
{
    public RestoreProductQueryHandler(
        IServiceFactory services,
        IResultFactory<RestoreProductQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<RestoreProductQueryHandler> localizer)
        : base(services, resultFactory, mapper, null, localizer)
    {
    }

    public async Task<ResultDto<Guid>> Handle(RestoreProductQuery request, CancellationToken cancellationToken)
    {
        _logger?.LogInformation(_localizer?["Starting Restore"]!);

        var productService = _servicesFactory.GetService<IProductService>();
        var result = await productService.RestoreAsync(request.Id);

        return result == Guid.Empty
            ? Fail<Guid>("RestoredFailed")
            : Success(result, "Restored");
    }
}
