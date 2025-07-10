namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command.HandlerCommand;

public class UpdateProductCommandHandler : BaseHandler<UpdateProductCommandHandler>, IRequestHandler<UpdateProductCommand, ResultDto<Guid>>
{
    public UpdateProductCommandHandler
        (
           IServiceFactory serviceFactory,
           IResultFactory<UpdateProductCommandHandler> resultFactory,
           IMapper mapper,
           IStringLocalizer<UpdateProductCommandHandler> localizar
        )
        : base(serviceFactory, resultFactory, mapper,  localizer: localizar)
    {
    }

    public async Task<ResultDto<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var getProduct = _servicesFactory.GetService<IProductService>();
        var getCategory = _servicesFactory.GetService<ICategoryService>();

        var includeExpressions = ProductIncludeHelper.GetIncludeExpressions(new List<ProductInclude>
        {
            ProductInclude.Category,
            ProductInclude.Product_Price,
            ProductInclude.Product_UnitProfile
        });

        var product = await getProduct.GetByIdAsync(request.Dto.Id,  true, includeExpressions);
        
        if (product is null)
            throw new NotFoundException(nameof(Product), request.Dto.Id);

        var category = await getCategory.GetByIdAsync(request.Dto.CategoryId);
        if (category is null)
            throw new NotFoundException(nameof(Category), request.Dto.CategoryId);

        _mapper?.Map(request.Dto, product);

       await getProduct.UpdateAsync(product);

        return _resultFactory.Success(product.Id, "Updated");
    }
}
