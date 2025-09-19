using MarketPOS.API.Middlewares.Exceptions;
using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services.ProductAndNavigetionEntity;

public class ProductPriceService : GenericServiceCacheing<ProductPrice>, IProductPriceService
{
    private readonly string _cacheKeyPrefix;
    public ProductPriceService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<ProductPrice>> localizer,
        ILogger<ProductPriceService> logger, IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache)
    {
        _cacheKeyPrefix = typeof(ProductPriceService).Name;
    }
    public async Task<ProductPrice?> GetByProductIdAsync(Guid productId)
    {
        var cacheKey = _cache.BuildCacheKey(nameof(GetByProductIdAsync), _cacheKeyPrefix, productId);

        try
        {
            return await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching {Entity} with ProductId {ProductId} from DB", _cacheKeyPrefix, productId);

                var result = await _unitOfWork.Repository<IProductPriceRepo>().GetByProductIdAsync(productId);

                if (result == null)
                {
                    _logger.LogWarning("No {Entity} found with ProductId {ProductId}", _cacheKeyPrefix, productId);
                }

                return result;
            }, TimeSpan.FromMinutes(5));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByProductIdFailed"]);
            throw new ResultDtoException(_localizer["GetByProductIdFailed"], ex);
        }
    }

    public async Task<int> UpdateByProductIdAsync(ProductPrice productPrice)
    {
        var cacheKey = _cache.BuildCacheKey(nameof(GetByProductIdAsync), _cacheKeyPrefix, productPrice.ProductId);

        try
        {
            var existing = await _unitOfWork.Repository<IProductPriceRepo>().GetByProductIdAsync(productPrice.ProductId);

            if (existing == null)
            {
                _logger.LogWarning("Update failed. No {Entity} found with ProductId {ProductId}", _cacheKeyPrefix, productPrice.ProductId);
                throw new ResultDtoException(_localizer["UpdateFailed"]);
            }

            _unitOfWork.Repository<IProductPriceRepo>().Update(productPrice);
            await _unitOfWork.SaveChangesAsync();

            // ✅ بعد التحديث لازم نعمل invalidate للـ cache عشان ميكونش عندنا بيانات قديمة
            await _cache.RemoveAsync(cacheKey);
            _logger.LogInformation("{Entity} with ProductId {ProductId} updated and cache invalidated", _cacheKeyPrefix, productPrice.ProductId);

            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw new ResultDtoException(_localizer["UpdateFailed"], ex);
        }
    }

}
