using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.RepositoryInterfaces;

public interface IReadOnlyRepository<T> : IQueryableRepository<T>, IProjectableRepository<T> where T : class
{
}
