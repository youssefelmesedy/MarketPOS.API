using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;
public class SofteDeletedCategoryQueryHandler : BaseHandler<SofteDeletedCategoryQueryHandler>, IRequestHandler<SofteCategoryDeletedQuery, ResultDto<SofteDeleteDto>>
{
    public SofteDeletedCategoryQueryHandler(
        IServiceFactory servicesFactory,
        IResultFactory<SofteDeletedCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<SofteDeletedCategoryQueryHandler> localizer)
        : base(servicesFactory, resultFactory, mapper, localizer: localizer)
    { }

    public async Task<ResultDto<SofteDeleteDto>> Handle(SofteCategoryDeletedQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var category = await categoryService.GetByIdAsync(request.Id, true, includeSoftDeleted: true);
        if (category is null)
            return _resultFactory.Fail<SofteDeleteDto>("NotFound");

        if (category.IsDeleted)
            return _resultFactory.Fail<SofteDeleteDto>("SofteDeletedFailed");

        var result = await categoryService.SoftDeleteAsync(category);

        var mapping = _mapper?.Map<SofteDeleteDto>(result);
        if (mapping is null)
            return _resultFactory.Fail<SofteDeleteDto>("Mappingfailed");

        return _resultFactory.Success(mapping, "SofteDeleted");
    }
}
