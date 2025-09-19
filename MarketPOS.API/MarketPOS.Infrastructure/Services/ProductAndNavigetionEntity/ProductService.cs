using MarketPOS.API.Middlewares.Exceptions;
using MarketPOS.Application.InterfaceCacheing;
using Microsoft.Data.SqlClient;
using YourProjectNamespace.Application.Common.Exceptions;

namespace MarketPOS.Infrastructure.Services.ProductAndNavigetionEntity;
public class ProductService : GenericServiceCacheing<Product>, IProductService
{
    private readonly string _cacheKeyPrefix;

    public ProductService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ProductService> localizer,
        ILogger<ProductService> logger,
        IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache) 
    {
        _cacheKeyPrefix = typeof(ProductService).Name;
    }

    // ✅ Get All With Category
    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(
        Guid? categoryId = null,
        List<Func<IQueryable<Product>, IQueryable<Product>>>? includeExpression = null,
        bool includeSoftDelete = false)
    {
        string cacheKey = _cache.BuildCacheKey(nameof(GetAllWithCategoryAsync), _cacheKeyPrefix,
            categoryId, includeExpression, includeSoftDelete);

        try
        {
            return await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching {Entity} from DB", _cacheKeyPrefix);
                var result = await _unitOfWork.Repository<IProductRepo>().FindAsync(
                    p => categoryId == null || p.CategoryId == categoryId,
                    includeExpressions: includeExpression,
                    includeSoftDeleted: includeSoftDelete);

                return result ?? Enumerable.Empty<Product>();
            }, TimeSpan.FromMinutes(10));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetAllFailed"]);
            throw new ResultDtoException(_localizer["GetAllFailed"]);
        }
    }

    // ✅ Get By Name
    public async Task<IEnumerable<Product>> GetByNameAsync(
        string name,
        List<Func<IQueryable<Product>, IQueryable<Product>>> includes,
        bool includeSoftDelete = false)
    {
        string cacheKey = _cache.BuildCacheKey(nameof(GetByNameAsync), _cacheKeyPrefix, name.ToLower().Trim(), includeSoftDelete);

        try
        {
            return await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching {Entity} by Name {Name} from DB", _cacheKeyPrefix, name);

                var result = await _unitOfWork.Repository<IProductRepo>().FindAsync(
                    p => p.Name.ToLower().Trim() == name.ToLower().Trim(),
                    includeExpressions: includes,
                    includeSoftDeleted: includeSoftDelete);

                return result ?? Enumerable.Empty<Product>();
            }, TimeSpan.FromMinutes(5));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByNameFailed"]);
            throw new ResultDtoException(_localizer["GetByNameFailed"]);
        }
    }

    // ✅ Get Ingredient Ids by ProductId
    public async Task<List<Guid>> GetIngredientIdsByProductIdAsync(Guid productId)
    {
        string cacheKey = _cache.BuildCacheKey(nameof(GetIngredientIdsByProductIdAsync), _cacheKeyPrefix, productId);

        try
        {
            return await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                var repo = _unitOfWork.RepositoryEntity<ProductActiveIngredient>();
                var existing = await repo.FindAsync(pi => pi.ProductId == productId);

                return existing?.Select(i => i.ActiveIngredinentsId).ToList() ?? new List<Guid>();
            }, TimeSpan.FromMinutes(10));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetIngredientsFailed"]);
            throw new ResultDtoException(_localizer["GetIngredientsFailed"]);
        }
    }

    // ✅ Update Product Ingredients
    public async Task UpdateProductIngredientAsync(Product product, List<Guid> ingredients)
    {
        try
        {
            var existingIds = product.ProductIngredients.Select(i => i.ActiveIngredinentsId).ToList();

            var toRemove = product.ProductIngredients
                .Where(pi => !ingredients.Contains(pi.ActiveIngredinentsId))
                .ToList();

            foreach (var item in toRemove)
                product.ProductIngredients.Remove(item);

            var toAdd = ingredients
                .Where(id => !existingIds.Contains(id))
                .Select(id => new ProductActiveIngredient
                {
                    ProductId = product.Id,
                    ActiveIngredinentsId = id
                })
                .ToList();

            foreach (var item in toAdd)
                product.ProductIngredients.Add(item);

            await _unitOfWork.SaveChangesAsync();

            // invalidate cache
            await _cache.RemoveAsync(_cache.BuildCacheKey(nameof(GetIngredientIdsByProductIdAsync), _cacheKeyPrefix, product.Id));

            // Warm cache (اختياري)
            await GetIngredientIdsByProductIdAsync(product.Id);
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx &&
                                             sqlEx.Number == 547) // FK constraint violation
        {
            _logger.LogError(dbEx, "Foreign key constraint violated when updating product ingredients.");
            throw new BusinessException(_localizer["IngredientNotFound"]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw new ResultDtoException(_localizer["UpdateFailed"]);
        }
    }
}


