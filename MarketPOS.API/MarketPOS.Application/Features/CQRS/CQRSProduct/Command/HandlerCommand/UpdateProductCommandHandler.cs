using MarketPOS.Application.Services.InterfacesServices;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

public class UpdateProductCommandHandler
    : BaseHandler<UpdateProductCommandHandler>,
      IRequestHandler<UpdateProductCommand, ResultDto<Guid>>
{
    private readonly IAggregateService _services;

    public UpdateProductCommandHandler(
        IAggregateService services,
        IServiceFactory serviceFactory,// ✅ بدل 4 Services
        IResultFactory<UpdateProductCommandHandler> resultFactory,
        IMapper mapper,
        IStringLocalizer<UpdateProductCommandHandler> localizer
    ) : base(serviceFactory, resultFactory, mapper, localizer: localizer)
    {
        _services = services;
    }

    public async Task<ResultDto<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // ✅ Check duplicate
        var duplicateError = await CheckDuplicateAsync(request.Dto);
        if (duplicateError != null)
            return _resultFactory.Fail<Guid>(duplicateError);

        // ✅ Load Product
        var product = await LoadProductAsync(request.Dto.Id);
        if (product is null)
            return _resultFactory.Fail<Guid>("GetByIdFailed");

        // ✅ Business validation
        await EnsureCategoryExistsAsync(request.Dto.CategoryId);

        // ✅ Updates
        await UpdateProductInfoAsync(product, request.Dto);
        await UpdateProductPriceAsync(product, request.Dto);
        await UpdateProductUnitProfileAsync(product, request.Dto);
        await UpdateProductIngredientsAsync(product, request.Dto);

        return _resultFactory.Success(product.Id, "Updated");
    }

    #region Private Helpers
    private async Task<string?> CheckDuplicateAsync(UpdateProductDto dto)
    {
        var normalizedName = dto.Name.Trim().ToLower();
        var normalizedBarcode = dto.Barcode?.Trim().ToLower();

        // ✅ Check name duplication (excluding the same Id)
        var nameExists = await _services.ProductService.AnyAsync(p =>
            (p.Name.Trim().ToLower() == normalizedName) &&
            p.Id != dto.Id);

        if (nameExists)
            return "DuplicateProductName";

        // ✅ Check barcode duplication (excluding the same Id)
        if (!string.IsNullOrEmpty(normalizedBarcode))
        {
            var barcodeExists = await _services.ProductService.AnyAsync(p =>
                p.Barcode != null &&
                p.Barcode.Trim().ToLower() == normalizedBarcode &&
                p.Id != dto.Id);

            if (barcodeExists)
                return "DuplicateProductBarcode";
        }

        return null; // ✅ No duplication
    }
    private async Task<Product?> LoadProductAsync(Guid id)
    {
        var includes = ProductIncludeHelper.GetIncludeExpressions(
        [
            ProductInclude.Category,
            ProductInclude.Product_Price,
            ProductInclude.Product_UnitProfile,
            ProductInclude.Ingredinent
        ]);

        return await _services.ProductService.GetByIdAsync(id, tracking: true, includes, true);
    }

    private async Task EnsureCategoryExistsAsync(Guid categoryId)
    {
        var category = await _services.CategoryService.GetByIdAsync(categoryId);
        if (category is null)
            throw new NotFoundException(nameof(Category), categoryId);
    }

    private async Task UpdateProductInfoAsync(Product product, UpdateProductDto dto)
    {
        if (product.UpdateValues(dto.Name, dto.Barcode, dto.CategoryId, dto.ExpirationDate ?? DateTime.MinValue))
            await _services.ProductService.UpdateAsync(product);
    }

    private async Task UpdateProductPriceAsync(Product product, UpdateProductDto dto)
    {
        if (product.ProductPrice is null) return;

        if (product.ProductPrice.UpdateValues(dto.SalePrice, dto.PurchasePrice, dto.DiscountPercentageFromSupplier, "Youssef"))
            await _services.ProductPriceService.UpdateByProductIdAsync(product.ProductPrice);
    }

    private async Task UpdateProductUnitProfileAsync(Product product, UpdateProductDto dto)
    {
        if (product.ProductUnitProfile is null) return;

        if (product.ProductUnitProfile.UpdateValues(dto.LargeUnitName, dto.MediumUnitName, dto.SmallUnitName, dto.MediumPerLarge, dto.SmallPerMedium))
            await _services.ProductUnitProfileService.UpdateByProductIdAsync(product.ProductUnitProfile);
    }

    private async Task UpdateProductIngredientsAsync(Product product, UpdateProductDto dto)
    {
        if (dto.IngredinentId is null || !dto.IngredinentId.Any()) return;

        var currentIds = product.ProductIngredients.Select(i => i.ActiveIngredinentsId).ToList();

        bool isSame = currentIds.Count == dto.IngredinentId.Count
                      && !dto.IngredinentId.Except(currentIds).Any();

        if (!isSame)
            await _services.ProductService.UpdateProductIngredientAsync(product, dto.IngredinentId);
    }

    #endregion
}
