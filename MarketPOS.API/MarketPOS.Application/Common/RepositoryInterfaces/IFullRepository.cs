using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.Common.RepositoryInterfaces;

public interface IFullRepository<T> : IReadOnlyRepository<T>, IWritableRepository<T> where T : class
{
}
