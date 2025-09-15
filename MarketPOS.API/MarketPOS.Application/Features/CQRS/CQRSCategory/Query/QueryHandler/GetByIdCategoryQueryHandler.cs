using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;
public class GetByIdCategoryQueryHandler : BaseHandler<GetByIdCategoryQueryHandler>,IRequestHandler<GetByIdCategoryQuery, ResultDto<CategoryDetalisDto>>
{
    public GetByIdCategoryQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<GetByIdCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<GetByIdCategoryQueryHandler> localizer,
        ILocalizationPostProcessor localizationPostProcessor) : base(serviceFactory, resultFactory, mapper, localizer : localizer, localizationPostProcessor : localizationPostProcessor)
    {
    }
    public async Task<ResultDto<CategoryDetalisDto>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();
       
        var data = await categoryService.GetByIdAsync(request.Id, true, null,request.SoftDeleted);

        if (data is null)
            return _resultFactory.Fail<CategoryDetalisDto>("GetByIdFailed");

        var mappeing = _mapper?.Map<CategoryDetalisDto>(data);
        if (mappeing is null)
            return _resultFactory.Fail<CategoryDetalisDto>("Mappingfailed");

        var localizer = _localizationPostProcessor.Apply(mappeing);

        return _resultFactory.Success(localizer, "Success");
    }
}
