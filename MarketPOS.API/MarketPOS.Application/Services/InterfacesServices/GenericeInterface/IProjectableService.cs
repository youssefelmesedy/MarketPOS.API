namespace MarketPOS.Application.Services.InterfacesServices.GenericeInterface;
using System.Linq.Expressions;

public interface IProjectableService<TEntity> where TEntity : class
{
    Task<List<TResult>> GetAllAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null);

    Task<TResult?> GetByIdAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false);

    Task<IEnumerable<TResult>> FindProjectedAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null);

    Task<(IEnumerable<TResult> Data, int TotalCount)> GetPagedProjectedAsync<TResult>(
        IMapper mapper,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false);
}

