namespace MarketPOS.Application.Common.Interfaces;

public interface IQueryableRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
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
        bool applyIncludes = true);

    Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true);

    Task<IEnumerable<T>> GetAllIncludingSoftDeletedAsync();
}

