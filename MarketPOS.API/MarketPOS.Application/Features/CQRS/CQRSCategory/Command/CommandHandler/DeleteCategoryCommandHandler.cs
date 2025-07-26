using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command.CommandHandler;
public class DeleteCategoryCommandHandler : BaseHandler<DeleteCategoryCommandHandler>, IRequestHandler<DeleteCategoryCommand, ResultDto<Guid>>
{
    public DeleteCategoryCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<DeleteCategoryCommandHandler> resultFactory,
        IStringLocalizer<DeleteCategoryCommandHandler> serviceLocalizer)
        : base(serviceFactory, resultFactory, localizer: serviceLocalizer)
    { }

    public async Task<ResultDto<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var service = _servicesFactory.GetService<ICategoryService>();

        var category = await service.GetByIdAsync(request.Id);
        if  (category is null) 
            throw new NotFoundException($"Not Found Category with Id: {request.Id}");

         await service.RemoveAsync(category);

        return _resultFactory.Success<Guid>(request.Id, "Deleted");
    }
}
