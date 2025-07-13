namespace Market.POS.Infrastructure.Repositories;

public class BaseBuildeQuery<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public BaseBuildeQuery(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    protected IQueryable<T> BuildQuery(
        Expression<Func<T, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        IQueryable<T> query = _dbSet;

        if (!tracking)
            query = query.AsNoTracking();

        if (!includeSoftDeleted && typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var isNotDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var softDeleteLambda = Expression.Lambda<Func<T, bool>>(isNotDeleted, parameter);
            query = query.Where(softDeleteLambda);
        }

        if (predicate is not null)
            query = query.Where(predicate);

        if (applyIncludes && includeExpressions is not null)
        {
            foreach (var include in includeExpressions)
                query = include(query);
        }

        return query;
    }
}
