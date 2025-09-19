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
        var getCategory = _servicesFactory.GetService<ICategoryService>();
        var productService = _servicesFactory.GetService<IProductService>();

        var category = await getCategory.GetByIdAsync(request.Dto.CategoryId);

        var existProductName = await productService.FindAsync(p => p.Name.Trim().ToLower() == request.Dto.Name.Trim().ToLower()
                                                                  || p.Barcode == request.Dto.Barcode);
        if (existProductName.Any())
            return _resultFactory.Fail<Guid>($"DuplicateProductName: \n {existProductName.Select(p => p.Id).First()}");

        if (category == null || category.IsDeleted)
            throw new NotFoundException(nameof(Category), request.Dto.CategoryId);

        var newproduct = _mapper?.Map<Product>(request.Dto);
        if (newproduct is null)
            return _resultFactory.Fail<Guid>("Mappingfailed");


        await productService.AddAsync(newproduct);

        return _resultFactory.Success(newproduct.Id, "Created");

    }
}
