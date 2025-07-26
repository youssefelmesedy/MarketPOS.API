using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHaandler;
public class GetAllCategoryQueryHandler : BaseHandler<GetAllCategoryQueryHandler>,IRequestHandler<GetAllCategoryQuery, ResultDto<List<CategoryDetalisDto>>>
{
    public GetAllCategoryQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<GetAllCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetAllCategoryQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : 
        base(serviceFactory, resultFactory, mapper, localizer : localizer, localizationPostProcessor : localizationPostProcessor)
    {
    }

    public async Task<ResultDto<List<CategoryDetalisDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var data = await categoryService.GetAllAsync<CategoryDetalisDto>(_mapper!, null, false, null, request.SoftDelete);

        var localized = _localizationPostProcessor.Apply<CategoryDetalisDto>(data);

        // Convert to IEnumerable if needed
        return _resultFactory.Success(localized.ToList(), "Success");
    }
}
