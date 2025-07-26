using Microsoft.EntityFrameworkCore.Query;

namespace MarketPOS.Application.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<Func<IQueryable<T>, IQueryable<T>>> IncludeExpressions { get; } = new();
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByDescending { get; protected set; }
    public int? Take { get; protected set; }
    public int? Skip { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }

    public bool IsTracking { get; protected set; } = false;
    public bool IncludeSoftDeleted { get; protected set; } = false;

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
        => Includes.Add(includeExpression);

    protected void AddInclude(Func<IQueryable<T>, IQueryable<T>> includeFunc)
        => IncludeExpressions.Add(includeFunc);

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        => OrderBy = orderBy;

    protected void ApplyOrderByDescending(Func<IQueryable<T>, IOrderedQueryable<T>> orderByDesc)
        => OrderByDescending = orderByDesc;

    protected void EnableTracking()
        => IsTracking = true;

    protected void EnableSoftDeleted(bool sofetDelete)
        => IncludeSoftDeleted = sofetDelete;
}
