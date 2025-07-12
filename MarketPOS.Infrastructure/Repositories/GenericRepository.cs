namespace Market.POS.Infrastructure.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    private IQueryable<T> BuildQuery(
     Expression<Func<T, bool>>? predicate = null,
     bool tracking = false,
     List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
     bool includeSoftDeleted = false)
    {
        IQueryable<T> query = _dbSet;

        // Disable tracking if requested
        if (!tracking)
            query = query.AsNoTracking();

        // Apply soft delete filter if entity derives from BaseEntity
        if (!includeSoftDeleted && typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var isNotDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var softDeleteLambda = Expression.Lambda<Func<T, bool>>(isNotDeleted, parameter);

            query = query.Where(softDeleteLambda);
        }

        // Apply custom predicate filter
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        // Apply include expressions for navigation properties
        if (includeExpressions is not null)
        {
            foreach (var include in includeExpressions)
            {
                query = include(query);
            }
        }

        return query;
    }

    public async Task<T?> GetByIdAsync(Guid id, bool tracking = false, List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null, bool includeSoftDeleted = false)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, "Id");
        var equals = Expression.Equal(property, Expression.Constant(id));
        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        return await BuildQuery(lambda, tracking, includeExpressions, includeSoftDeleted).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool tracking = false, List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null, bool includeSoftDeleted = false)
    {
        return await BuildQuery(null, tracking, includeExpressions, includeSoftDeleted).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool tracking = false, List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null, bool icludeSofteDelete = false)
    {
        return await BuildQuery(predicate, tracking, includeExpressions, icludeSofteDelete).ToListAsync();
    }

    public async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null)
    {
        IQueryable<T> query = BuildQuery(filter, tracking, includeExpressions);

        if (orderBy != null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var total = await query.CountAsync();
        var data = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    // Optional: Method to include soft-deleted entities
    public async Task<IEnumerable<T>> GetAllIncludingSoftDeletedAsync()
    {
        return await BuildQuery(null, false, null, includeSoftDeleted: true).ToListAsync();
    }
}

