using AutoMapper.QueryableExtensions;
using MarketPOS.Infrastructure.Context.Persistence;
using MarketPOS.Infrastructure.TrackingServicesMiddleware;
using System.Diagnostics;

namespace MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

public class GenericRepository<TEntity> : BaseBuildeQuery<TEntity>, IFullRepository<TEntity> where TEntity : class
{
    private readonly ILogger<GenericRepository<TEntity>> _logger;
    public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<TEntity>> logger) : base(context)
    {
        ServiceTracker.Add(typeof(GenericRepository<TEntity>).Name);
        _logger = logger;
    }

    #region ProjectableRepository Methods
    public async Task<(IEnumerable<TResult> Data, int TotalCount)> GetPagedProjectedAsync<TResult>(
    IMapper mapper,
    int pageIndex,
    int pageSize,
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
    bool ascending = true,
    bool tracking = false,
    List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
    bool includeSoftDeleted = false,
    bool applyIncludes = false)
    {
        var query = BuildQuery(filter, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);

        var total = await query.CountAsync();
        var data = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<TResult>(mapper.ConfigurationProvider)
            .ToListAsync();

        return (data, total);
    }

    public async Task<List<TResult>> GetAllProjectedListAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null, // Optional if needed
        bool includeSoftDeleted = false,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = false) // Important: false when using ProjectTo
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Repository - GetAllAsync executing EF query");

        var query = BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);

        stopwatch.Stop();
        _logger.LogInformation("Repository - EF query finished in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<TResult?> GetProjectedByIdAsync<TResult>(
    IMapper mapper,
    Expression<Func<TEntity, bool>> predicate,
    bool tracking = false,
    List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
    bool includeSoftDeleted = false,
    bool applyIncludes = false)
    {
        var query = BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, null, applyIncludes);
        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TResult>> FindProjectedAsync<TResult>(
    IMapper mapper,
    Expression<Func<TEntity, bool>> predicate,
    bool tracking = false,
    List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
    bool includeSoftDeleted = false,
     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
    bool applyIncludes = false)
    {
        var query = BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);
        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool includeSofteDelete = false)
    {
        var query = BuildQuery(predicate, includeSoftDeleted: includeSofteDelete);
        return await query.AnyAsync(predicate);
    }
    #endregion

    #region QueryableRepository Methods
    public async Task<IEnumerable<TEntity>> GetAllAsync(
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Repository - GetAllAsync executing EF query");

        var query = await BuildQuery(null, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes).ToListAsync();

        stopwatch.Stop();
        _logger.LogInformation("Repository - EF query finished in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

        return query.ToList();
    }

    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, "Id");
        var equals = Expression.Equal(property, Expression.Constant(id));
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

        return await BuildQuery(lambda, tracking, includeExpressions, includeSoftDeleted, null, applyIncludes).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true)
    {
        return await BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes).ToListAsync();
    }
    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true)
    {
        return await BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        IQueryable<TEntity> query = BuildQuery(filter, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);

        //if (ordering != null)
        //    query = ascending ? query.OrderBy(ordering) : query.OrderByDescending(orderBy);

        var total = await query.CountAsync();
        var data = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }
    #endregion

    #region WritableRepository Methods
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
    #endregion
}
