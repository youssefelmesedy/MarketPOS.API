using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class ProductUnitProfileService : GenericService<ProductUnitProfile>, IProductUnitProfileService
{
    public readonly IProductUnitProfileRepo _productUnitProfileRepo;
    public ProductUnitProfileService(IReadOnlyRepository<ProductUnitProfile> query, IFullRepository<ProductUnitProfile> write, IUnitOfWork unitOfWork, IStringLocalizer<GenericService<ProductUnitProfile>> localizer, ILogger<ProductUnitProfileService> logger, IProductUnitProfileRepo productUnitProfileRepo)
        : base(query, write, unitOfWork, localizer, logger)
    {
        _productUnitProfileRepo = productUnitProfileRepo;
    }

    public async Task<ProductUnitProfile> GetByProductIdAsync(Guid productId)
    {
        try
        {
            var result = await _productUnitProfileRepo.GetByProductIdAsync(productId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByProductIdFailed"]);
            throw;
        }
    }

    public async Task<int> UpdateByProductIdAsync(ProductUnitProfile productUnitProfile)
    {
        try
        {
            var existing = await _productUnitProfileRepo.GetByProductIdAsync(productUnitProfile.ProductId);

            _write.Update(existing); // أو _productPriceRepository.Update(existing)
            await _unitOfWork.SaveChangesAsync();
            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw;
        }
    }
}
