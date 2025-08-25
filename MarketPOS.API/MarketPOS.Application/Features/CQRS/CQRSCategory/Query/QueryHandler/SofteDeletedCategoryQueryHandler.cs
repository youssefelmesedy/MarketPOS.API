using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;
public class SofteDeletedCategoryQueryHandler : BaseHandler<SofteDeletedCategoryQueryHandler>, IRequestHandler<SofteCategoryDeletedQuery, ResultDto<SofteDeleteDto>>
{
    public SofteDeletedCategoryQueryHandler(
        IServiceFactory servicesFactory,
        IResultFactory<SofteDeletedCategoryQueryHandler> resultFactory,
        IStringLocalizer<SofteDeletedCategoryQueryHandler> localizer) : base(servicesFactory, resultFactory, localizer : localizer)
    { }

    public async Task<ResultDto<SofteDeleteDto>> Handle(SofteCategoryDeletedQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var category = await categoryService.GetByIdAsync(request.Id);
        if (category is null)
            return _resultFactory.Fail<SofteDeleteDto>("NotFound");

        var result = await categoryService.SoftDeleteAsync(category);

        var mapping = _mapper?.Map<SofteDeleteDto>(result); 
        if (mapping is null)
            return _resultFactory.Fail<SofteDeleteDto>("MappingFailed");

        return _resultFactory.Success(mapping, "SofteDeleted");
    }
}
