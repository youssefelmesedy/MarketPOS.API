using Market.Domain.Entitys;
using MarketPOS.Application.Specifications;

namespace Market.POS.Infrastructure.Repositories;

public class SpecificationEvaluator<T> : ISpecificationEvaluator<T> where T : class
{
    public IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        if (!spec.IsTracking)
            query = query.AsNoTracking();

        if (!spec.IncludeSoftDeleted && typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var isNotDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var softDeleteLambda = Expression.Lambda<Func<T, bool>>(isNotDeleted, parameter);
            query = query.Where(softDeleteLambda);
        }

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        foreach (var include in spec.Includes)
            query = query.Include(include);

        foreach (var include in spec.IncludeExpressions)
            query = include(query);

        if (spec.OrderBy is not null)
            query = spec.OrderBy(query);
        else if (spec.OrderByDescending is not null)
            query = spec.OrderByDescending(query);

        if (spec.IsPagingEnabled)
        {
            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);
            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);
        }

        return query;
    }
}

