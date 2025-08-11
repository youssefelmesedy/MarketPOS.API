using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;

public class ProductPriceService : GenericService<ProductPrice>, IProductPriceService
{
    public ProductPriceService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<ProductPrice>> localizer,
        ILogger<ProductPriceService> logger) 
        : base(unitOfWork, localizer, logger)
    {
    }

    public async Task<ProductPrice> GetByProductIdAsync(Guid productId)
    {
        try
        {
            var result = await _unitOfWork.Repository<IProductPriceRepo>().GetByProductIdAsync(productId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByProductIdFailed"]);
            throw;
        }
    }

    public async Task<int> UpdateByProductIdAsync(ProductPrice productPrice)
    {
        try
        {
            var existing = await _unitOfWork.Repository<IProductPriceRepo>().GetByProductIdAsync(productPrice.ProductId);

            _unitOfWork.Repository<IProductPriceRepo>().Update(existing); // أو _productPriceRepository.Update(existing)
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
