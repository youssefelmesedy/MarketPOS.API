using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.Common.RepositoryInterfaces;

public interface IReadOnlyRepository<T> : IQueryableRepository<T>, IProjectableRepository<T> where T : class
{
}
