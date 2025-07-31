using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.RepositoryInterfaces;

public interface IFullRepository<T> : IReadOnlyRepository<T>, IWritableRepository<T> where T : class
{
}
