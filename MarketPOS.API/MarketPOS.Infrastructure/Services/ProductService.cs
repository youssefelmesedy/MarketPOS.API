using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Specifications;
using Microsoft.Data.SqlClient;
using YourProjectNamespace.Application.Common.Exceptions;

namespace MarketPOS.Infrastructure.Services;

public class ProductService : GenericService<Product>, IProductService
{

    public ProductService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ProductService> localizer,
        ILogger<ProductService> logger,
        IGenericCache cache) // Inject Cache
        : base(unitOfWork, localizer, logger)
    {
    }

    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(
        Guid? categoryId = null,
        List<Func<IQueryable<Product>, IQueryable<Product>>>? includeExpression = null,
        bool includeSoftDelete = false)
    {
        string cacheKey = $"Products_Category_{categoryId ?? Guid.Empty}";

        // جرب تجيب من الكاش
        var cached = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
        if (cached is not null)
        {
            _logger.LogInformation("Cache Hit for {CacheKey}", cacheKey);
            return cached;
        }

        try
        {
            var result = await _unitOfWork.Repository<IProductRepo>().FindAsync(
                p => categoryId == null || p.CategoryId == categoryId,
                includeExpressions: includeExpression,
                includeSoftDeleted: includeSoftDelete);

            // خزّن في الكاش 10 دقائق
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetAllFailed"]);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(
        string name,
        List<Func<IQueryable<Product>, IQueryable<Product>>> includes,
        bool includeSoftDelete = false)
    {
        string cacheKey = $"Product_Name_{name.ToLower().Trim()}";

        var cached = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
        if (cached is not null)
        {
            _logger.LogInformation("Cache Hit for {CacheKey}", cacheKey);
            return cached;
        }

        try
        {
            var result = await _unitOfWork.Repository<IProductRepo>().FindAsync(
                p => p.Name.ToLower().Trim() == name.ToLower().Trim(),
                includeExpressions: includes,
                includeSoftDeleted: includeSoftDelete);

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByNameFailed"]);
            throw;
        }
    }

    public async Task<List<Guid>> GetIngredientIdsByProductIdAsync(Guid productId)
    {
        string cacheKey = $"Product_Ingredients_{productId}";

        var cached = await _cache.GetAsync<List<Guid>>(cacheKey);
        if (cached is not null)
        {
            _logger.LogInformation("Cache Hit for {CacheKey}", cacheKey);
            return cached;
        }

        try
        {
            var repo = _unitOfWork.RepositoryEntity<ProductActiveIngredient>();
            var existing = await repo.FindAsync(pi => pi.ProductId == productId);

            var result = existing.Select(i => i.ActiveIngredinentsId).ToList();

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByIdFailed"]);
            throw;
        }
    }

    public async Task<IEnumerable<Product>?> GetProductbyCategoryIdSpce(ISpecification<Product> specification)
    {
        try
        {
            // 1️⃣ - اعمل مفتاح مميز للكاش مبني على الـ Specification
            var cacheKey = $"Products_Spec_{specification.ToCacheKey()}";

            // 2️⃣ - جرّب تجيب من الكاش
            var cachedResult = await _cache.GetAsync<IEnumerable<Product>>(cacheKey);
            if (cachedResult != null)
            {
                _logger.LogInformation(_localizer["Success"] + $" (Cache Hit): Key = {cacheKey}");
                return cachedResult;
            }

            // 3️⃣ - لو مش موجود في الكاش .. استعلم من DB
            var result = await _unitOfWork.Repository<IProductRepo>().GetBySpecificationAsync(specification);

            // 4️⃣ - خزن النتيجة في الكاش لفترة محددة
            if (result != null && result.Any())
            {
                await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByIdFailed"]);
            throw;
        }
    }

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

            await _cache.RemoveAsync($"Product_Ingredients_{product.Id}");
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx &&
                                             sqlEx.Number == 547) // 547 = FK constraint violation
        {
            _logger.LogError(dbEx, "Foreign key constraint violated when updating product ingredients.");
            throw new BusinessException(_localizer["IngredientNotFound"]);
            // BusinessException هي Exception انت معرفها عشان تترجمها في Middleware لرسالة واضحة
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw;
        }
    }
}

