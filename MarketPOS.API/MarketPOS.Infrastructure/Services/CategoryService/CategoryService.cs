using MarketPOS.API.Middlewares.Exceptions;
using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services.CategoryService;
public class CategoryService : GenericServiceCacheing<Category>, ICategoryService
{
    private readonly string _cacheKeyPrefix;
    public CategoryService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<Category>> localizer,
        ILogger<CategoryService> logger,
        IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache)
    {
        _cacheKeyPrefix = typeof(CategoryService).Name;
    }
    public async Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSoftDelete = false)
    {
        var cacheKey = _cache.BuildCacheKey(
            nameof(GetByNameAsync),
            _cacheKeyPrefix,
            name.ToLower().Trim(),
            includeSoftDelete);

        try
        {
            return await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation(
                    "Fetching {Entity} with Name {Name} from DB",
                    _cacheKeyPrefix,
                    name);

                return await _unitOfWork
                    .Repository<ICategoryRepo>()
                    .FindAsync(
                        c => c.Name.ToLower().Trim() == name.ToLower().Trim(),
                        includeSoftDeleted: includeSoftDelete);
            }, TimeSpan.FromMinutes(5));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while fetching {Entity} with Name {Name}. CacheKey: {CacheKey}",
                _cacheKeyPrefix,
                name,
                cacheKey);

            // نرمي exception مخصص بدل العام
            throw new ResultDtoException(
                $"Failed to fetch {_cacheKeyPrefix} by name '{name}'", ex);
        }
    }



}
