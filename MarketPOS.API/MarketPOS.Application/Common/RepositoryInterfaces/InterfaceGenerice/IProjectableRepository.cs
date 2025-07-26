namespace MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

public interface IProjectableRepository<T> where T : class
{
    Task<List<TResult>> GetProjectedListAsync<TResult>(
        IMapper mapper,
        Expression<Func<T, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null,
        bool applyIncludes = false);

    Task<TResult?> GetProjectedByIdAsync<TResult>(
        IMapper mapper,
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = false);

    Task<IEnumerable<TResult>> FindProjectedAsync<TResult>(
        IMapper mapper,
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
         Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null,
        bool applyIncludes = false);

    Task<(IEnumerable<TResult> Data, int TotalCount)> GetPagedProjectedAsync<TResult>(
        IMapper mapper,
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = false);
}

