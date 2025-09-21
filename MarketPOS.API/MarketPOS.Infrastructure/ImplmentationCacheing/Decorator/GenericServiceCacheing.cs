using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Infrastructure.TrackingServicesMiddleware;
using System.Security.Cryptography;
using System.Text;

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
        ServiceTracker.Add(typeof(TEntity).Name);
        _cacheKeyPrefix = typeof(TEntity).Name;
    }

    #region Queryable Methods (Cached)

    public override async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        var cacheKey = BuildNormalizedCacheKey<TEntity>(
            nameof(GetPagedAsync), filter, includeExpressions, orderBy, pageIndex, pageSize, ascending);

        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Fetching paged {Entity} from DB", _cacheKeyPrefix);
            return await base.GetPagedAsync(pageIndex, pageSize, filter, orderBy, ascending,
                tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        }, TimeSpan.FromMinutes(2));
    }

    public override async Task<TEntity?> GetByIdAsync(Guid id, bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false, bool applyIncludes = true)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{nameof(GetByIdAsync)}:{id}:{includeExpressions?.Count ?? 0}:{includeSoftDeleted}";

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
        var cacheKey = BuildNormalizedCacheKey<TEntity>(
            nameof(GetAllAsync), null, includeExpressions, ordering);

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
        var cacheKey = BuildNormalizedCacheKey<TEntity>(
            nameof(FindAsync), predicate, includeExpressions, ordering);

        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Finding {Entity} with predicate from DB", _cacheKeyPrefix);
            return await base.FindAsync(predicate, tracking, includeExpressions, includeSoftDeleted, ordering);
        }, TimeSpan.FromMinutes(2));
    }

    // 🚨 AnyAsync = من غير كاش
    public override async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool includeSoftDeleted = false)
    {
        _logger.LogInformation("Checking existence of {Entity} from DB", _cacheKeyPrefix);
        return await base.AnyAsync(predicate, includeSoftDeleted);
    }

    #endregion

    #region Projectable Methods (Cached)

    public override async Task<List<TResult>> GetAllAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null)
    {
        var cacheKey = BuildNormalizedCacheKey<TResult>(
            nameof(GetAllAsync), predicate, includeExpressions, ordering);

        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Fetching all {Entity} projected to {Result} from DB",
                _cacheKeyPrefix, typeof(TResult).Name);

            return await base.GetAllAsync<TResult>(
                mapper, predicate, tracking, includeExpressions, includeSoftDeleted, ordering);
        }, TimeSpan.FromMinutes(2));
    }

    public override async Task<TResult?> GetByIdProjectedAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
        where TResult : class
    {
        var cacheKey = BuildNormalizedCacheKey<TResult>(
            nameof(GetByIdProjectedAsync), predicate, includeExpressions);

        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Fetching projected {Entity} to {Result} by predicate from DB",
                _cacheKeyPrefix, typeof(TResult).Name);

            return await base.GetByIdProjectedAsync<TResult>(
                mapper, predicate, tracking, includeExpressions, includeSoftDeleted);
        }, TimeSpan.FromMinutes(5));
    }

    public override async Task<IEnumerable<TResult>> FindProjectedAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null)
    {
        var cacheKey = BuildNormalizedCacheKey<TResult>(
            nameof(FindProjectedAsync), predicate, includeExpressions, ordering);

        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Finding projected {Entity} to {Result} from DB",
                _cacheKeyPrefix, typeof(TResult).Name);

            return await base.FindProjectedAsync<TResult>(
                mapper, predicate, tracking, includeExpressions, includeSoftDeleted, ordering);
        }, TimeSpan.FromMinutes(2));
    }

    public override async Task<(IEnumerable<TResult> Data, int TotalCount)> GetPagedProjectedAsync<TResult>(
        IMapper mapper,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        var cacheKey = BuildNormalizedCacheKey<TResult>(
            nameof(GetPagedProjectedAsync), filter, includeExpressions, ordering, pageIndex, pageSize, ascending);

        return await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            _logger.LogInformation("Fetching paged projected {Entity} to {Result} from DB",
                _cacheKeyPrefix, typeof(TResult).Name);

            return await base.GetPagedProjectedAsync<TResult>(
                mapper, pageIndex, pageSize, filter, ordering, ascending, tracking, includeExpressions, includeSoftDeleted);
        }, TimeSpan.FromMinutes(2));
    }

    #endregion

    #region Writable Methods (Clear Cache After Changes)

    public override async Task AddAsync(TEntity entity)
    {
        await base.AddAsync(entity);
        await _cache.RemoveByPrefixAsync(_cacheKeyPrefix); // invalidate all keys
    }

    public override async Task UpdateAsync(TEntity entity)
    {
        await base.UpdateAsync(entity);
        await _cache.RemoveByPrefixAsync(_cacheKeyPrefix);
    }

    public override async Task RemoveAsync(TEntity entity)
    {
        await base.RemoveAsync(entity);
        await _cache.RemoveByPrefixAsync(_cacheKeyPrefix);
    }

    public override async Task<TEntity> SoftDeleteAsync(TEntity entity)
    {
        var result = await base.SoftDeleteAsync(entity);
        await _cache.RemoveByPrefixAsync(_cacheKeyPrefix);
        return result;
    }

    public override async Task<TEntity> RestoreAsync(TEntity entity)
    {
        var result = await base.RestoreAsync(entity);
        await _cache.RemoveByPrefixAsync(_cacheKeyPrefix);
        return result;
    }

    #endregion

    #region Key Builder (With Hashing)

    private string BuildNormalizedCacheKey<TResult>(
        string methodName,
        Expression<Func<TEntity, bool>>? predicate = null,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        int? pageIndex = null,
        int? pageSize = null,
        bool? ascending = null)
    {
        var keyBuilder = new StringBuilder($"{_cacheKeyPrefix}:{methodName}:{typeof(TResult).Name}");

        if (predicate != null)
            keyBuilder.Append($":predicate:{HashKey(predicate.ToString())}");

        if (includeExpressions != null && includeExpressions.Any())
            keyBuilder.Append($":includes:{HashKey(string.Join(",", includeExpressions.Select(x => x.ToString())))}");

        if (ordering != null)
            keyBuilder.Append($":ordering:{HashKey(ordering.ToString()!)}");

        if (pageIndex.HasValue && pageSize.HasValue)
            keyBuilder.Append($":page:{pageIndex}:{pageSize}:{ascending}");

        return keyBuilder.ToString();
    }

    private string HashKey(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    #endregion
}
