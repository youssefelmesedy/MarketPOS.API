using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MarketPOS.Infrastructure.Services;
public class GenericService<TEntity> :
            IQueryableService<TEntity>,
            IProjectableService<TEntity>,
            IWritableService<TEntity> where TEntity : class
{
    private readonly IQueryableRepository<TEntity> _query;
    private readonly IProjectableRepository<TEntity> _projectable;
    private readonly IWritableRepository<TEntity> _writable;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<GenericService<TEntity>> _localizer;
    private readonly ILogger _logger;

    public GenericService(
        IUnitOfWork unitOfWork,
        ILogger logger,
        IQueryableRepository<TEntity> query,
        IProjectableRepository<TEntity> projectable,
        IWritableRepository<TEntity> writable,
        IStringLocalizer<GenericService<TEntity>> localizer)
    {
        _query = query;
        _projectable = projectable;
        _writable = writable;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _localizer = localizer;
    }

    #region Queryable Methods

    public async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        try
        {
            return await _query.GetPagedAsync(pageIndex, pageSize, filter, orderBy, ascending, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetPagedFailed"]);
            throw;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        try
        {
            return await _query.GetAllAsync(tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetAllFailed"]);
            throw;
        }
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        try
        {
            return await _query.GetByIdAsync(id, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByIdFailed"]);
            throw;
        }
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        try
        {
            return await _query.FindAsync(predicate, tracking, includeExpressions, includeSoftDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["FindFailed"]);
            throw;
        }
    }

    #endregion

    #region Projectable Methods

    public async Task<List<TResult>> GetAllAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        try
        {
            return await _projectable.GetProjectedListAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetAllFailed"]);
            throw;
        }
    }

    public async Task<TResult?> GetByIdAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        try
        {
            return await _projectable.GetProjectedByIdAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByIdFailed"]);
            throw;
        }
    }

    public async Task<IEnumerable<TResult>> FindProjectedAsync<TResult>(
        IMapper mapper,
        Expression<Func<TEntity, bool>> predicate,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        try
        {
            return await _projectable.FindProjectedAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["FindFailed"]);
            throw;
        }
    }

    public async Task<(IEnumerable<TResult> Data, int TotalCount)> GetPagedProjectedAsync<TResult>(
        IMapper mapper,
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        try
        {
            return await _projectable.GetPagedProjectedAsync<TResult>(mapper, pageIndex, pageSize, filter, orderBy, ascending, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetPagedFailed"]);
            throw;
        }
    }

    #endregion

    #region Writable Methods

    public async Task AddAsync(TEntity entity)
    {
        try
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedAt = DateTime.Now;
                baseEntity.CreatedBy = "System";
            }

            await _writable.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["CreateFailed"]);
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
                baseEntity.ModifiedBy = "System";
            }

            _writable.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw;
        }
    }

    public async Task RemoveAsync(TEntity entity)
    {
        try
        {
            _writable.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["DeleteFailed"]);
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
                _writable.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }

            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["SofteDeletedFailed"]);
            throw;
        }
    }

    public async Task<Guid> RestoreAsync(Guid id)
    {
        try
        {
            var entity = await _query.GetByIdAsync(id, true, null, includeSoftDeleted: true, applyIncludes: true);

            if (entity == null)
                throw new NotFoundException(typeof(TEntity).Name, id);

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = false;
                baseEntity.DeletedAt = null;
                _writable.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return baseEntity.Id;
            }

            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["RestoreFailed"]);
            throw;
        }
    }

    #endregion
}

