using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Command.CommandHandler;
public class CreateCategoryCommandHandler : BaseHandler<CreateCategoryCommandHandler>, IRequestHandler<CreateCategoryCommand, ResultDto<Guid>>
{
    public CreateCategoryCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<CreateCategoryCommandHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<CreateCategoryCommandHandler> localizer)
        : base(serviceFactory, resultFactory, mapper, localizer: localizer)
    { }
    public async Task<ResultDto<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = _servicesFactory.GetService<ICategoryService>();

        var category = _mapper?.Map<Category>(request.Dto);
        if (category is null)
            return _resultFactory.Fail<Guid>("MappingFiled");

        var existCategoryName = await serviceCategory.FindAsync(c => c.Name.ToLower().Trim() ==
                                                                     category.Name.ToLower().Trim());
        if (existCategoryName.Any())
            return _resultFactory.Fail<Guid>($"DuplicateCategoryName: \n {existCategoryName.Select(c => c.Id).First()}");

        await serviceCategory.AddAsync(category);

        return _resultFactory.Success<Guid>(category.Id, "Created");
    }
}
