namespace MarketPOS.Application.Specifications;

public interface ISpecificationEvaluator<T>
{
    IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification);
}

