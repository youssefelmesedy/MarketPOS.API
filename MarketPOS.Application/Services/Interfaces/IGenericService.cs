namespace MarketPOS.Application.Services.Interfaces;
public interface IGenericService<TEntity> where TEntity : class
{
    Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null);

    Task<IEnumerable<TEntity>> GetAllAsync(
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null, bool includeSoftDeleted = false);

    Task<TEntity?> GetByIdAsync(
        Guid id,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null, bool includeSoftDeleted = false);

    Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null);

    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task<TEntity> SoftDeleteAsync(TEntity entity);
    Task<Guid> RestoreAsync(Guid id);

}
