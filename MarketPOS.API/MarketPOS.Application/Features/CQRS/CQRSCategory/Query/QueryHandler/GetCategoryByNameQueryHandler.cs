using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHaandler;
public class GetCategoryByNameQueryHandler : BaseHandler<GetCategoryByNameQueryHandler>,IRequestHandler<GetCategoryName, ResultDto<IEnumerable<CategoryDetalisDto>>>
{
    public GetCategoryByNameQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<GetCategoryByNameQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetCategoryByNameQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : base(serviceFactory, resultFactory, mapper, localizer: localizer, localizationPostProcessor : localizationPostProcessor)
    {
    }

    public async Task<ResultDto<IEnumerable<CategoryDetalisDto>>> Handle(GetCategoryName request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var data = await categoryService.GetByNameAsync(request.CategoryName!, request.IncludSofteDelete);
        if (!data.Any())
            return _resultFactory.Fail<IEnumerable<CategoryDetalisDto>>("GetByNameFailed");

        var mappeing = _mapper?.Map<IEnumerable<CategoryDetalisDto>>(data);
        if (mappeing is null)
            return _resultFactory.Fail<IEnumerable<CategoryDetalisDto>>("Mappingfailed");

        var localized = _localizationPostProcessor.Apply(mappeing);

        return _resultFactory.Success(localized, "Success");
    }
}
