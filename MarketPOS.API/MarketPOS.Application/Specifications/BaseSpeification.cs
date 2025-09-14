using Microsoft.EntityFrameworkCore.Query;
using System.Security.Cryptography;
using System.Text;

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

    public string ToCacheKey()
    {
        var criteria = Criteria?.Body.ToString() ?? "NoCriteria";
        var orderBy = OrderBy != null ? "OrderBy" : "NoOrderBy";
        var orderByDesc = OrderByDescending != null ? "OrderByDesc" : "NoOrderByDesc";
        var includes = string.Join(",", Includes.Select(i => i.Body.ToString()));
        var paging = IsPagingEnabled ? $"Skip={Skip}_Take={Take}" : "NoPaging";
        var tracking = IsTracking ? "Tracking" : "NoTracking";
        var softDelete = IncludeSoftDeleted ? "WithSoftDelete" : "WithoutSoftDelete";

        var rawKey = $"Spec:{typeof(T).Name}|{criteria}|{orderBy}|{orderByDesc}|{includes}|{paging}|{tracking}|{softDelete}";

        // هنعمل Hash بالـ SHA256
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
        var hashString = Convert.ToBase64String(hashBytes);

        return $"Spec_{typeof(T).Name}_{hashString}";
    }
}
