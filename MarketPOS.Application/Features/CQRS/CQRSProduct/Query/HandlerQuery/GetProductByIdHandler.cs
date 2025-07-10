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

public class GetProductByIdHandler : BaseHandler<GetProductByIdHandler>, IRequestHandler<GetProductByIdQuery, ResultDto<ProductDetailsDto>>
{
    public GetProductByIdHandler(
        IServiceFactory factoryService,
        IResultFactory<GetProductByIdHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetProductByIdHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : base(factoryService, resultFactory, mapper, null, localizer, localizationPostProcessor)
    { }

    public async Task<ResultDto<ProductDetailsDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var productServic = _servicesFactory.GetService<IProductService>();

        var includeExpressions = ProductIncludeHelper.GetIncludeExpressions(new List<ProductInclude>
        {
            ProductInclude.Category,
            ProductInclude.Product_Inventorie,
            ProductInclude.Product_Price,
            ProductInclude.Product_UnitProfile
        });

        var product = await productServic
            .GetByIdAsync(request.Id, true, includeExpressions: includeExpressions , request.SoftDelete);

        if (product is null)
            return _resultFactory.Fail<ProductDetailsDto>("NotFound");

        var mappeing = _mapper?.Map<ProductDetailsDto>(product);
        if(mappeing is null)
            return _resultFactory.Fail<ProductDetailsDto>("MappingError");

        var localizer = _localizationPostProcessor.Apply(mappeing);
        
        return _resultFactory.Success(localizer, "Success");
    }
}
