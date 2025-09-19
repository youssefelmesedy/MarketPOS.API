using MarketPOS.API.Middlewares.Exceptions;
using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services.ProductAndNavigetionEntity;
public class ProductUnitProfileService : GenericServiceCacheing<ProductUnitProfile>, IProductUnitProfileService
{
    private readonly string _cacheKeyPrefix;

    public ProductUnitProfileService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<ProductUnitProfile>> localizer,
        ILogger<ProductUnitProfileService> logger,
        IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache)
    {
        _cacheKeyPrefix = typeof(ProductUnitProfileService).Name;
    }

    public async Task<ProductUnitProfile> GetByProductIdAsync(Guid productId)
    {
        string cacheKey = _cache.BuildCacheKey(nameof(GetByProductIdAsync), _cacheKeyPrefix, productId);

        try
        {
            return await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching ProductUnitProfile with ProductId {ProductId} from DB", productId);
                var result = await _unitOfWork.Repository<IProductUnitProfileRepo>().GetByProductIdAsync(productId);
                if (result is null)
                    throw new ResultDtoException(_localizer["ProductUnitProfileNotFound"]);
                return result;
            }, TimeSpan.FromMinutes(5));
        }
        catch (ResultDtoException)
        {
            throw; // Already a ResultDtoException
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByProductIdFailed"]);
            throw new ResultDtoException(_localizer["GetByProductIdFailed"]);
        }
    }

    public async Task<int> UpdateByProductIdAsync(ProductUnitProfile productUnitProfile)
    {
        string cacheKey = _cache.BuildCacheKey(nameof(GetByProductIdAsync), _cacheKeyPrefix, productUnitProfile.ProductId);

        try
        {
            var existing = await _unitOfWork.Repository<IProductUnitProfileRepo>().GetByProductIdAsync(productUnitProfile.ProductId);
            if (existing is null)
                throw new ResultDtoException(_localizer["ProductUnitProfileNotFound"]); 

            _unitOfWork.Repository<IProductUnitProfileRepo>().Update(existing);
            await _unitOfWork.SaveChangesAsync();

            // إزالة الكاش بعد التحديث
            await _cache.RemoveAsync(cacheKey);

            return 1;
        }
        catch (ResultDtoException)
        {
            throw; // Already a ResultDtoException
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw new ResultDtoException(_localizer["UpdateFailed"]);
        }
    }
}

