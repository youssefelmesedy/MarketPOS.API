namespace MarketPOS.Infrastructure.Services;

public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork, ILogger logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null)
    {
        try
        {
            return await _repository.GetPagedAsync(pageIndex, pageSize, filter, orderBy, ascending, tracking, includeExpressions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPagedAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null, bool includeSoftDeleted = false)
    {
        try
        {
            return await _repository.GetAllAsync(tracking, includeExpressions, includeSoftDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool tracking = false, List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null, bool includeSoftDeleted = false)
    {
        try
        {
            return await _repository.GetByIdAsync(id, tracking, includeExpressions, includeSoftDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null)
    {
        try
        {
            return await _repository.FindAsync(predicate, tracking, includeExpressions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in FindAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task AddAsync(TEntity entity)
    {
        try
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedAt = DateTime.Now;
                baseEntity.CreatedBy = "System"; // Replace with actual user context if available
                await _repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.UpdatedAt = DateTime.Now;
                baseEntity.ModifiedBy = "System"; // Replace with actual user context if available
                _repository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task RemoveAsync(TEntity entity)
    {
        try
        {
            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RemoveAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }

    public async Task<TEntity> SoftDeleteAsync(TEntity entity)
    {
        try
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                baseEntity.DeletedAt = DateTime.Now;
                _repository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return entity;
            }

            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SoftDeleteAsync for entity {Entity}", typeof(TEntity).Name);
            throw;
        }
    }
    public async Task<Guid> RestoreAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id, true, null, true);

        if (entity is null)
            throw new NotFoundException(typeof(TEntity).Name, id);

        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = false;
            baseEntity.DeletedAt = null;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return baseEntity.Id;
        }

        return id;
    }

}
