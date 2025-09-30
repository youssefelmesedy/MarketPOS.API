namespace MarketPOS.Application.RepositoryInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    // Generic Repository
    IFullRepository<TEntity> RepositoryEntity<TEntity>() where TEntity : class;

    // Custom Repositories (لو حابب تعمل Repos مخصصة غير الـ Generic)
    TRepository Repository<TRepository>() where TRepository : class;

    // Save Changes
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Transactional Execution (ExecutionStrategy + inline transaction)
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default);

    // Manual TransactionScope (لو محتاج تحكم كامل)
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

