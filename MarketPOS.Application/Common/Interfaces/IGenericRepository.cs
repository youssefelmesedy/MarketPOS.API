namespace MarketPOS.Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(
       Guid id,
       bool tracking = false,
       List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null, bool includeSoftDeleted = false);

    Task<IEnumerable<T>> GetAllAsync(
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null, bool includeSoftDeleted = false);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null);

    Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null);

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    // ✅ NEW: Get all including soft-deleted entities
    Task<IEnumerable<T>> GetAllIncludingSoftDeletedAsync();
}

