using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class ProductUnitProfileService : GenericService<ProductUnitProfile>, IProductUnitProfileService
{
    public ProductUnitProfileService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<ProductUnitProfile>> localizer,
        ILogger<ProductUnitProfileService> logger)
        : base(unitOfWork, localizer, logger)
    {
    }

    public async Task<ProductUnitProfile> GetByProductIdAsync(Guid productId)
    {
        try
        {
            var result = await _unitOfWork.Repository<IProductUnitProfileRepo>().GetByProductIdAsync(productId);
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
            var existing = await _unitOfWork.Repository<IProductUnitProfileRepo>().GetByProductIdAsync(productUnitProfile.ProductId);

            _unitOfWork.Repository<IProductUnitProfileRepo>().Update(existing); // أو _productPriceRepository.Update(existing)
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
