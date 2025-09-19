using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace Market.POS.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction? _transaction;
    private readonly Hashtable _repositories = new Hashtable();

    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IFullRepository<TEntity> RepositoryEntity<TEntity>() where TEntity : class
    {
        var typeName = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            // حاول تنشئ instance من GenericRepository<TEntity>
            var repositoryType = typeof(GenericeRepository<>).MakeGenericType(typeof(TEntity));
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);

            if (repositoryInstance == null)
                throw new InvalidOperationException($"Could not create repository instance for entity type '{typeName}'.");

            _repositories.Add(typeName, repositoryInstance);
        }

        var repository = _repositories[typeName];
        if (repository is not IFullRepository<TEntity> typedRepository)
            throw new InvalidCastException($"Repository instance is not of the expected type IFullRepository<{typeName}>.");

        return typedRepository;
    }


    public TRepository Repository<TRepository>() where TRepository : class
    {
        var typeName = typeof(TRepository).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            // نحصل على الريبو من DI container
            var repositoryInstance = _serviceProvider.GetService<TRepository>();
            if (repositoryInstance == null)
                throw new InvalidOperationException($"Repository of type '{typeName}' is not registered in the DI container.");

            _repositories.Add(typeName, repositoryInstance);
        }

        var repo = _repositories[typeName];
        if (repo is not TRepository typedRepo)
            throw new InvalidCastException($"Repository instance stored for '{typeName}' cannot be cast to '{typeof(TRepository)}'.");

        return typedRepo;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction to commit.");

        await _transaction.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction to rollback.");

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}


