using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command.CommandHandler;
public class UpdateCategoryCommandHandler : BaseHandler<UpdateCategoryCommandHandler>,
    IRequestHandler<UpdateCategoryCommand, ResultDto<Guid>>
{
    public UpdateCategoryCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<UpdateCategoryCommandHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<UpdateCategoryCommandHandler> localizer)
        : base(serviceFactory, resultFactory, mapper, localizer: localizer)
    { }

    public async Task<ResultDto<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = _servicesFactory.GetService<ICategoryService>();

        var esistCategory = await serviceCategory.GetByIdAsync(request.dto.Id, true, includeSoftDeleted: request.SofteDelete);
        if (esistCategory == null)
            throw new NotFoundException(nameof(Category), request.dto.Id);

        var category = _mapper?.Map(request.dto, esistCategory);
        if (category is null)
            return _resultFactory.Fail<Guid>("MappingFiled");

        var existCategoryName = await serviceCategory.FindAsync(c => c.Name.ToLower().Trim() ==
                                                                     category.Name.ToLower().Trim() &&
                                                                     c.Id != category.Id);

        if (existCategoryName.Any())
         return _resultFactory.Fail<Guid>("DuplicateCategoryName");

        await serviceCategory.UpdateAsync(category);

        return _resultFactory.Success<Guid>(category.Id, "Updated");
    }
}
