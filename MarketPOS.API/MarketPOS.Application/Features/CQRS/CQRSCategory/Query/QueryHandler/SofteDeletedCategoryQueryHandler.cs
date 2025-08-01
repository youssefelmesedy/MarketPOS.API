﻿using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;
public class SofteDeletedCategoryQueryHandler : BaseHandler<SofteDeletedCategoryQueryHandler>, IRequestHandler<SofteCategoryDeletedQuery, ResultDto<Guid>>
{
    public SofteDeletedCategoryQueryHandler(
        IServiceFactory servicesFactory,
        IResultFactory<SofteDeletedCategoryQueryHandler> resultFactory,
        IStringLocalizer<SofteDeletedCategoryQueryHandler> localizer) : base(servicesFactory, resultFactory, localizer : localizer)
    { }

    public async Task<ResultDto<Guid>> Handle(SofteCategoryDeletedQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var category = await categoryService.GetByIdAsync(request.Id);
        if (category is null)
            return _resultFactory.Fail<Guid>("NotFound");

        var result = await categoryService.SoftDeleteAsync(category);

        return _resultFactory.Success(result.Id, "SofteDeleted");
    }
}
