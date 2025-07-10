using AutoMapper;
using MarketPOS.Application.Common.HandlerBehaviors;
using MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;
using MarketPOS.Application.Services.Interfaces;
using MarketPOS.Design.FactoryResult;
using MarketPOS.Design.FactoryServices;
using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.CategoryDto;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHaandler;

public class GetAllCategoryQueryHandler : BaseHandler<GetAllCategoryQueryHandler>,IRequestHandler<GetAllCategoryQuery, ResultDto<IEnumerable<CategoryDetalisDto>>>
{
    public GetAllCategoryQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<GetAllCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetAllCategoryQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : base(serviceFactory, resultFactory, mapper, localizer : localizer, localizationPostProcessor : localizationPostProcessor)
    {
    }

    public async Task<ResultDto<IEnumerable<CategoryDetalisDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var data = await categoryService.GetAllAsync(false,null,request.SoftDelete);
        if (data is null)
            return _resultFactory.Fail<IEnumerable<CategoryDetalisDto>>("NotFound");

        var mappeing = _mapper?.Map<IEnumerable<CategoryDetalisDto>>(data);
        if (mappeing is null)
            return _resultFactory.Fail<IEnumerable<CategoryDetalisDto>>("Mappingfailed");

        var Localizer = _localizationPostProcessor.Apply(mappeing);

        return _resultFactory.Success(Localizer, "Success");
    }
}
