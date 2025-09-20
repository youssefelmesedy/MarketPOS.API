using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command.HandlerCommand;
public class CreateProductCommandHandler : BaseHandler<CreateProductCommandHandler>, IRequestHandler<CreateProductCommand, ResultDto<Guid>>
{
    public CreateProductCommandHandler
        (
        IServiceFactory serviceFactory,
        IResultFactory<CreateProductCommandHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<CreateProductCommandHandler> localizer)
       : base(serviceFactory, resultFactory, mapper, localizer: localizer)
    {
    }
    public async Task<ResultDto<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryService = _servicesFactory.GetService<ICategoryService>();
        var productService = _servicesFactory.GetService<IProductService>();

        var category = await categoryService.GetByIdAsync(request.Dto.CategoryId);
        if (category == null || category.IsDeleted)
            throw new NotFoundException(nameof(Category), request.Dto.CategoryId);

        var newName = request.Dto.Name.Trim().ToLower();
        var newBarcode = request.Dto.Barcode?.Trim().ToLower();

        if (await productService.AnyAsync(p => p.Name.ToLower().Trim() == newName))
            return _resultFactory.Fail<Guid>("DuplicateProductName");

        if (!string.IsNullOrEmpty(newBarcode) &&
            await productService.AnyAsync(p => p.Barcode != null && p.Barcode.ToLower().Trim() == newBarcode))
            return _resultFactory.Fail<Guid>("DuplicateBarcode");

        var newProduct = _mapper!.Map<Product>(request.Dto);
        if (newProduct == null)
            return _resultFactory.Fail<Guid>("MappingFailed");

        await productService.AddAsync(newProduct);

        return _resultFactory.Success(newProduct.Id, "Created");
    }
}
