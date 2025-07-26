namespace MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

public interface IWritableRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}

