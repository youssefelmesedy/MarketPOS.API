namespace MarketPOS.Application.RepositoryInterfaces;
public interface IUnitOfWork : IDisposable
{
    public IFullRepository<TEntity> RepositoryEntity<TEntity>() where TEntity : class;
    TRepository Repository<TRepository>() where TRepository : class;
    Task<int> SaveChangesAsync();

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}

