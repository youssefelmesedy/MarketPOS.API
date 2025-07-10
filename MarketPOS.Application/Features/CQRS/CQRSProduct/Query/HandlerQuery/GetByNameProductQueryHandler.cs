using AutoMapper;
using Market.POS.Application.Services.Interfaces;
using MarketPOS.Application.Common.HandlerBehaviors;
using MarketPOS.Application.Common.Helpers.IncludeHalpers;
using MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;
using MarketPOS.Design.FactoryResult;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.ProductDto;
using MarketPOS.Shared.Eunms.ProductEunms;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query.HandlerQuery;

public class GetByNameProductQueryHandler : BaseHandler<GetByNameProductQueryHandler>, IRequestHandler<GetByNameProductQuery, ResultDto<IEnumerable<SomeFeaturesProductDto>>>
{

    public GetByNameProductQueryHandler(
        IServiceFactory serviceFactoory, 
        IResultFactory<GetByNameProductQueryHandler> resultFactory,
        IMapper mapper, IStringLocalizer<GetByNameProductQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) 
        : base(serviceFactoory, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<IEnumerable<SomeFeaturesProductDto>>> Handle(GetByNameProductQuery request, CancellationToken cancellationToken)
    {
        var service = _servicesFactory.GetService<IProductService>();

        var includes = ProductIncludeHelper.GetIncludeExpressions
        (
            new List<ProductInclude>
            {
                ProductInclude.Category,
                ProductInclude.Product_Price,
                ProductInclude.Product_Inventorie
            }
        );

        var data = await service.GetByNameAsync(request.Name!, includes);
        if (data is null || !data.Any())
            return _resultFactory.Fail<IEnumerable<SomeFeaturesProductDto>>("GetByNameFailed");
        

        var mappeing = _mapper?.Map<IEnumerable<SomeFeaturesProductDto>>(data);
        if (mappeing is null)
            return _resultFactory.Fail<IEnumerable<SomeFeaturesProductDto>>("MappingError");

        var localize = _localizationPostProcessor.Apply(mappeing);

        return _resultFactory.Success(localize, "Success");
    }
}
