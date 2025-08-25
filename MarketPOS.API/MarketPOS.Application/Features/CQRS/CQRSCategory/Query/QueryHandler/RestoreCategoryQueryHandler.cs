using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;
public class RestoreCategoryQueryHandler : BaseHandler<RestoreCategoryQueryHandler>,
    IRequestHandler<RestoreCategoryQuery, ResultDto<RestorDto>>
{
    public RestoreCategoryQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<RestoreCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<RestoreCategoryQueryHandler> localizer) : base(serviceFactory, resultFactory, mapper, localizer : localizer)
    {
    }

    public async Task<ResultDto<RestorDto>> Handle(RestoreCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var category = await categoryService.GetByIdAsync(request.Id, includeSoftDeleted: true);

        if (category is null)
            return _resultFactory.Fail<RestorDto>("GetByIdFailed");

        if (!category.IsDeleted)
            return _resultFactory.Fail<RestorDto>("RestoreFailed");

        var result = await categoryService.RestoreAsync(category);

        var mapping = _mapper?.Map<RestorDto>(result);
        if (mapping is null)
            return _resultFactory.Fail<RestorDto>("MappingFailed");

        var localizedResult = _localizationPostProcessor.Apply(mapping);

        return _resultFactory.Success(localizedResult, "RestoredBy");
    }
}
