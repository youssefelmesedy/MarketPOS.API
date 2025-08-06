using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;
public class RestoreCategoryQueryHandler : BaseHandler<RestoreCategoryQueryHandler>, IRequestHandler<RestoreCategoryQuery, ResultDto<SofteDeleteAndRestorDto>>
{
    public RestoreCategoryQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<RestoreCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<RestoreCategoryQueryHandler> localizer) : base(serviceFactory, resultFactory, mapper, localizer : localizer)
    {
    }

    public async Task<ResultDto<SofteDeleteAndRestorDto>> Handle(RestoreCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var category = await categoryService.GetByIdAsync(request.Id, includeSoftDeleted: true);

        if (category is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("GetByIdFailed");

        if (!category.IsDeleted)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("RestoreFailed");

        var result = await categoryService.RestoreAsync(category);

        var mapping = _mapper?.Map<SofteDeleteAndRestorDto>(result);
        if (mapping is null)
            return _resultFactory.Fail<SofteDeleteAndRestorDto>("MappingFailed");

        var localizedResult = _localizationPostProcessor.Apply(mapping);

        return _resultFactory.Success(localizedResult, "Restored");
    }
}
