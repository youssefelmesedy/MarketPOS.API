using MarketPOS.Application.Common.RepositoryInterfaces;
using MarketPOS.Application.Services.InterfacesServices;

namespace MarketPOS.Infrastructure.Services;
public class GenericService<TEntity> :IFullService<TEntity> where TEntity : class
{
    protected readonly IReadOnlyRepository<TEntity> _query;
    protected readonly IFullRepository<TEntity> _write;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IStringLocalizer<GenericService<TEntity>> _localizer;
    protected readonly ILogger _logger;

    public GenericService(IReadOnlyRepository<TEntity> query, IFullRepository<TEntity> write, IUnitOfWork unitOfWork, IStringLocalizer<GenericService<TEntity>> localizer, ILogger logger)
    {
        _query = query;
        _write = write;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _logger = logger;
    }

    #region Queryable Methods

    public async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false,
        bool applyIncludes = true)
    {
        try
        {
            return await _query.GetPagedAsync(pageIndex, pageSize, filter,ordering : orderBy, ascending: ascending, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
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
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool applyIncludes = true)
    {
        try
        {
            return await _query.GetAllAsync(tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);
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
        bool includeSoftDeleted = false,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null)
    {
        try
        {
            return await _query.FindAsync(predicate, tracking, includeExpressions, includeSoftDeleted, ordering);
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
        bool includeSoftDeleted = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null)
    {
        try
        {
            return await _query.GetProjectedListAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, 
                                 ordering, applyIncludes: false);
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
            return await _query.GetProjectedByIdAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
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
        bool includeSoftDeleted = false, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null)
    {
        try
        {
            return await _query.FindProjectedAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes: false);
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
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? ordering = null,
        bool ascending = true,
        bool tracking = false,
        List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>? includeExpressions = null,
        bool includeSoftDeleted = false)
    {
        try
        {
            return await _query.GetPagedProjectedAsync<TResult>(mapper, pageIndex, pageSize, filter, ordering, ascending, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
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
            if (entity is Product product)
                product.InitializeChildEntityinCreate();

            if (entity is Category category)
                category.InitializeBasePropertyInCreate();

            await _write.AddAsync(entity);
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
            if (entity is Product product)
                product.InitializeChildEntityinUpdate();

            if (entity is Category category)
                category.InitializeBaseInUpDate();

            _write.Update(entity);
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
            _write.Remove(entity);
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
                baseEntity.DeleteBy = "Youssef";
                baseEntity.IsDeleted = true;
                baseEntity.DeletedAt = DateTime.Now;
                baseEntity.RestorAt = baseEntity.RestorAt;
                baseEntity.RestorBy = baseEntity.RestorBy;

                _write.Update(entity);
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
                baseEntity.DeletedAt = baseEntity.DeletedAt;
                baseEntity.DeleteBy = baseEntity.DeleteBy;

                baseEntity.RestorAt = DateTime.Now;
                baseEntity.RestorBy = "Youssef";

                _write.Update(entity);
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

