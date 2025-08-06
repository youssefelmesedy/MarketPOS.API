using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
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
        var productService = _servicesFactory.GetService<IProductService>();
        var categoryService = _servicesFactory.GetService<ICategoryService>();
        var priceService = _servicesFactory.GetService<IProductPriceService>();
        var unittProfileService = _servicesFactory.GetService<IProductUnitProfileService>();

        var normalizedName = request.Dto.Name.Trim().ToLower();
        var normalizedBarcode = request.Dto.Barcode?.Trim();

        var existProduct = await productService.FindAsync(p =>
            (p.Name.Trim().ToLower() == normalizedName || p.Barcode == normalizedBarcode) &&
            p.Id != request.Dto.Id);

        if (existProduct.Any())
            return _resultFactory.Success<Guid>(existProduct.Select(p => p.Id).First() ,"DuplicateProductName");

        var includes = ProductIncludeHelper.GetIncludeExpressions(
        [
            ProductInclude.Category,
            ProductInclude.Product_Price,
            ProductInclude.Product_UnitProfile
        ]);

        var product = await productService.GetByIdAsync(request.Dto.Id, true, includes)
            ?? throw new NotFoundException(nameof(Product), request.Dto.Id);

        await EnsureCategoryExists(categoryService, request.Dto.CategoryId);

        await HandleProductInfoUpdateAsync(productService, product, request.Dto);
        await HandleProductPriceUpdateAsync(priceService, product, request.Dto);
        await HandleProductUnitProfileUpdateAsync(unittProfileService, product, request.Dto);

        return _resultFactory.Success(product.Id, "Updated");
    }


    /// <summary>
    /// Ensures that the specified category exists in the system.
    /// Throws a NotFoundException if the category is not found.
    /// </summary>
    /// <param name="categoryService">The category service to retrieve the category.</param>
    /// <param name="categoryId">The ID of the category to check.</param>
    /// <exception cref="NotFoundException">Thrown when the category is not found.</exception>

    private async Task EnsureCategoryExists(ICategoryService categoryService, Guid categoryId)
    {
        var category = await categoryService.GetByIdAsync(categoryId);
        if (category is null)
            throw new NotFoundException(nameof(Category), categoryId);
    }

    /// <summary>
    /// Updates the product price entity if any relevant values have changed.
    /// Applies the update through the IProductPriceService.
    /// </summary>
    /// <param name="priceService">The service responsible for managing product prices.</param>
    /// <param name="product">The product containing the existing price data.</param>
    /// <param name="dto">The DTO containing the new price values.</param>

    private async Task HandleProductPriceUpdateAsync(
        IProductPriceService priceService,
        Product product,
        UpdateProductDto dto)
    {
        if (product.ProductPrice is null)
            return;

        var modified = product.ProductPrice.UpdateValues(
            dto.SalePrice,
            dto.PurchasePrice,
            dto.DiscountPercentageFromSupplier,
            "Youssef");

        if (modified)
            await priceService.UpdateByProductIdAsync(product.ProductPrice);
    }

    /// <summary>
    /// Updates product information (name, barcode, category, expiration date)
    /// if any of the values have changed, using the IProductService.
    /// </summary>
    /// <param name="productService">The service responsible for updating products.</param>
    /// <param name="product">The existing product entity to be updated.</param>
    /// <param name="dto">The DTO containing the new product values.</param>
    private async Task HandleProductInfoUpdateAsync(
        IProductService productService,
        Product product,
        UpdateProductDto dto)
    {
        var modified = product.UpdateValues(
            dto.Name,
            dto.Barcode,
            dto.CategoryId, 
            dto.ExpirationDate ?? DateTime.MinValue);

        if (modified)
            await productService.UpdateAsync(product);
    }

    /// <summary>
    /// Handles updating the <see cref="ProductUnitProfile"/> of a given <see cref="Product"/> 
    /// if the profile exists and any of its unit-related values have changed.
    /// </summary>
    /// <param name="productService">The service responsible for updating product unit profiles.</param>
    /// <param name="product">The product containing the current unit profile.</param>
    /// <param name="dto">The DTO containing the updated unit profile values.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>

    private async Task HandleProductUnitProfileUpdateAsync(
        IProductUnitProfileService productService,
        Product product,
        UpdateProductDto dto)
    {
        if(product.ProductUnitProfile is null)
            return;

        var modified = product.ProductUnitProfile.UpdateValues(
            dto.LargeUnitName,
            dto.MediumUnitName,
            dto.SmallUnitName,
            dto.MediumPerLarge,
            dto.SmallPerMedium);

        if (modified)
            await productService.UpdateByProductIdAsync(product.ProductUnitProfile);
    }
}
