﻿using AutoMapper.QueryableExtensions;

namespace Market.POS.Infrastructure.Repositories;

public class GenericeRepository<T> : BaseBuildeQuery<T>,
    IQueryableRepository<T>,
    IProjectableRepository<T>,
    IWritableRepository<T> where T : class
{
    public GenericeRepository(ApplicationDbContext context) : base(context)
    {
    }

    // ProjectableRepository Methods
    public async Task<(IEnumerable<TResult> Data, int TotalCount)> GetPagedProjectedAsync<TResult>(
    IMapper mapper,
    int pageIndex,
    int pageSize,
    Expression<Func<T, bool>>? filter = null,
    Expression<Func<T, object>>? orderBy = null,
    bool ascending = true,
    bool tracking = false,
    List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
    bool includeSoftDeleted = false,
    bool applyIncludes = false)
    {
        var query = BuildQuery(filter, tracking, includeExpressions, includeSoftDeleted, applyIncludes);

        if (orderBy != null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var total = await query.CountAsync();
        var data = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<TResult>(mapper.ConfigurationProvider)
            .ToListAsync();

        return (data, total);
    }

    public async Task<List<TResult>> GetProjectedListAsync<TResult>(
        IMapper mapper,
        Expression<Func<T, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null, // Optional if needed
        bool includeSoftDeleted = false,
        bool applyIncludes = false) // Important: false when using ProjectTo
    {
        var query = BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<TResult?> GetProjectedByIdAsync<TResult>(
    IMapper mapper,
    Expression<Func<T, bool>> predicate,
    bool tracking = false,
    List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
    bool includeSoftDeleted = false,
    bool applyIncludes = false)
    {
        var query = BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TResult>> FindProjectedAsync<TResult>(
    IMapper mapper,
    Expression<Func<T, bool>> predicate,
    bool tracking = false,
    List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
    bool includeSoftDeleted = false,
    bool applyIncludes = false)
    {
        var query = BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).ToListAsync();
    }

    // QueryableRepository Methods
    public async Task<IEnumerable<T>> GetAllAsync(
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        return await BuildQuery(null, tracking, includeExpressions, includeSoftDeleted, applyIncludes).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, "Id");
        var equals = Expression.Equal(property, Expression.Constant(id));
        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        return await BuildQuery(lambda, tracking, includeExpressions, includeSoftDeleted, applyIncludes).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        return await BuildQuery(predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes).ToListAsync();
    }

    public async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<T>, IQueryable<T>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        IQueryable<T> query = BuildQuery(filter, tracking, includeExpressions, includeSoftDeleted, applyIncludes);

        if (orderBy != null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var total = await query.CountAsync();
        var data = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<IEnumerable<T>> GetAllIncludingSoftDeletedAsync()
    {
        return await BuildQuery(null, false, null, includeSoftDeleted: true).ToListAsync();
    }

    // WritableRepository Methods
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
}
