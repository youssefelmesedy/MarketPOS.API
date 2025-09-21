namespace MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

public interface IQueryableRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true);

    Task<TEntity?> GetByIdAsync(Guid id,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool includeSofteDelete = false);

    Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true);
}

