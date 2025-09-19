namespace MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

public interface IQueryableRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null,
        bool applyIncludes = true);

    Task<T?> GetByIdAsync(Guid id,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null,
        bool applyIncludes = true);

    Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true);
}

