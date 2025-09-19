using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Design.Decorator;

public class GenericServiceCacheing<TEntity> : GenericService<TEntity>, IFullService<TEntity>
    where TEntity : class
{
    private readonly string _cacheKeyPrefix;

    public GenericServiceCacheing(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<TEntity>> localizer,
        ILogger<GenericServiceCacheing<TEntity>> logger,
        IGenericCache cacheService)
        : base(unitOfWork, localizer, logger, cacheService)
    {
        _cacheKeyPrefix = typeof(TEntity).Name;
    }

    #region Queryable Methods (Cached)

    public override async Task<TEntity?> GetByIdAsync(Guid id, bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false, bool applyIncludes = true)
    {
        var cacheKey = _cache.BuildCacheKey(nameof(GetByIdAsync), _cacheKeyPrefix, id, includeExpressions?.GetHashCode() ?? 0, includeSoftDeleted);
        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Fetching {Entity} Id {Id} from DB", _cacheKeyPrefix, id);
            return await base.GetByIdAsync(id, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        }, TimeSpan.FromMinutes(5));
    }

    public override async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true)
    {
        var cacheKey = _cache.BuildCacheKey(nameof(GetAllAsync), _cacheKeyPrefix, includeExpressions?.GetHashCode() ?? 0, includeSoftDeleted, ordering?.GetHashCode() ?? 0);
        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Fetching all {Entity} from DB", _cacheKeyPrefix);
            return await base.GetAllAsync(tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);
        }, TimeSpan.FromMinutes(2));
    }

    public override async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null)
    {
        var cacheKey = _cache.BuildCacheKey(nameof(FindAsync), _cacheKeyPrefix, predicate.GetHashCode(), includeExpressions?.GetHashCode() ?? 0);
        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Finding {Entity} with predicate from DB", _cacheKeyPrefix);
            return await base.FindAsync(predicate, tracking, includeExpressions, includeSoftDeleted, ordering);
        }, TimeSpan.FromMinutes(2));
    }

    #endregion

    #region Writable Methods (Clear Cache After Changes)

    public override async Task AddAsync(TEntity entity)
    {
        await base.AddAsync(entity);
        await _cache.RemoveAsync(_cacheKeyPrefix);
    }

    public override async Task UpdateAsync(TEntity entity)
    {
        await base.UpdateAsync(entity);
        await _cache.RemoveAsync(_cacheKeyPrefix);
    }

    public override async Task RemoveAsync(TEntity entity)
    {
        await base.RemoveAsync(entity);
        await _cache.RemoveAsync(_cacheKeyPrefix);
    }

    public override async Task<TEntity> SoftDeleteAsync(TEntity entity)
    {
        var result = await base.SoftDeleteAsync(entity);
        await _cache.RemoveAsync(_cacheKeyPrefix);
        return result;
    }

    public override async Task<TEntity> RestoreAsync(TEntity entity)
    {
        var result = await base.RestoreAsync(entity);
        await _cache.RemoveAsync(_cacheKeyPrefix);
        return result;
    }

    #endregion
}
