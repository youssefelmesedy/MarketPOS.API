using Microsoft.EntityFrameworkCore.Query;

namespace MarketPOS.Application.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<Func<IQueryable<T>, IQueryable<T>>> IncludeExpressions { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByDescending { get; }
    int? Take { get; }
    int? Skip { get; }
    bool IsPagingEnabled { get; }

    // 🆕 New Properties
    bool IsTracking { get; }
    bool IncludeSoftDeleted { get; }

    string ToCacheKey();
}


