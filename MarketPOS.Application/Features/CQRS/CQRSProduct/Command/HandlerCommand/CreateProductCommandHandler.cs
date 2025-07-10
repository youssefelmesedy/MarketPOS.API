namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command.HandlerCommand;
public class CreateProductCommandHandler : BaseHandler<CreateProductCommandHandler>, IRequestHandler<CreateProductCommand, ResultDto<Guid>>
{
    
    public CreateProductCommandHandler
        (
        IServiceFactory serviceFactory,
        IResultFactory<CreateProductCommandHandler>resultFactory,
        IMapper mapper,
        IStringLocalizer<CreateProductCommandHandler> localizer)
       : base(serviceFactory, resultFactory, mapper, localizer: localizer)
    {
    }

    public async Task<ResultDto<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var getCategory = _servicesFactory.GetService<ICategoryService>();

        var category = await getCategory.GetByIdAsync(request.Dto.CategoryId);
        if (category == null || category.IsDeleted)
            throw new NotFoundException(nameof(Category), request.Dto.CategoryId);

        var product = _mapper?.Map<Product>(request.Dto);
        if (product is null)
            return _resultFactory.Fail<Guid>("Mappingfailed");

        var productService = _servicesFactory.GetService<IProductService>();

        await productService.AddAsync(product);

        return _resultFactory.Success(product.Id, "Created");

    }
}
