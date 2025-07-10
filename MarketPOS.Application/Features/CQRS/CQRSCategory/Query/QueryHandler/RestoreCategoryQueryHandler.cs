namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query.QueryHandler;

public class RestoreCategoryQueryHandler : BaseHandler<RestoreCategoryQueryHandler>, IRequestHandler<RestoreCategoryQuery, ResultDto<Guid>>
{
    public RestoreCategoryQueryHandler(
        IServiceFactory serviceFactory,
        IResultFactory<RestoreCategoryQueryHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<RestoreCategoryQueryHandler> localizer) : base(serviceFactory, resultFactory, mapper, localizer : localizer)
    {
    }

    public async Task<ResultDto<Guid>> Handle(RestoreCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();

        var result = await categoryService.RestoreAsync(request.Id);
        if (result == Guid.Empty)
            return _resultFactory.Fail<Guid>("RestoreFailed");

        return _resultFactory.Success(result, "Restored");
    }
}
