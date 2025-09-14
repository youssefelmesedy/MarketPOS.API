
using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services;
public class GenericService<TEntity> :IFullService<TEntity>, IReadOnlyService<TEntity> where TEntity : class
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IStringLocalizer<GenericService<TEntity>> _localizer;
    protected readonly ILogger _logger;
    protected readonly IGenericCache _cache;

    public GenericService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<TEntity>> localizer,
        ILogger logger,
        IGenericCache cache = null!)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _logger = logger;
        _cache = cache;
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
        var cachKey = $"Page_CachKey_({pageIndex})_({pageSize})";
        try
        {

            return await _unitOfWork.RepositoryEntity<TEntity>().GetPagedAsync(pageIndex, pageSize, filter,ordering : orderBy, ascending: ascending, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
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
            return await _unitOfWork.RepositoryEntity<TEntity>().GetAllAsync(tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes);
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
            return await _unitOfWork.RepositoryEntity<TEntity>().GetByIdAsync(id, tracking, includeExpressions, includeSoftDeleted, applyIncludes);
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
            return await _unitOfWork.RepositoryEntity<TEntity>().FindAsync(predicate, tracking, includeExpressions, includeSoftDeleted, ordering);
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
            return await _unitOfWork.RepositoryEntity<TEntity>().GetProjectedListAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, 
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
            return await _unitOfWork.RepositoryEntity<TEntity>().GetProjectedByIdAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
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
            return await _unitOfWork.RepositoryEntity<TEntity>().FindProjectedAsync<TResult>(mapper, predicate, tracking, includeExpressions, includeSoftDeleted, ordering, applyIncludes: false);
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
            return await _unitOfWork.RepositoryEntity<TEntity>().GetPagedProjectedAsync<TResult>(mapper, pageIndex, pageSize, filter, ordering, ascending, tracking, includeExpressions, includeSoftDeleted, applyIncludes: false);
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
            //if (entity is Product product)
            //    product.InitializeChildEntityinCreate();

            //if (entity is Category category)
            //    category.InitializeBasePropertyInCreate();

            if (entity is BaseEntity baseEntity)
                baseEntity.InitializeChildEntityinCreate();

            await _unitOfWork.RepositoryEntity<TEntity>().AddAsync(entity);
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
            //if (entity is Product product)
            //    product.InitializeChildEntityinUpdate();

            //if (entity is Category category)
            //    category.InitializeBaseInUpDate();

            if (entity is BaseEntity baseEntity)
                baseEntity.InitializeChildEntityinUpdate();

            _unitOfWork.RepositoryEntity<TEntity>().Update(entity);
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
            _unitOfWork.RepositoryEntity<TEntity>().Remove(entity);
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

                _unitOfWork.RepositoryEntity<TEntity>().Update(entity);
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

    public async Task<TEntity> RestoreAsync(TEntity entity)
    {
        try
        {

            if (entity == null)
                throw new NotFoundException(typeof(TEntity).Name);

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = false;
                baseEntity.DeletedAt = baseEntity.DeletedAt;
                baseEntity.DeleteBy = baseEntity.DeleteBy;

                baseEntity.RestorAt = DateTime.Now;
                baseEntity.RestorBy = "Youssef";

                _unitOfWork.RepositoryEntity<TEntity>().Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return entity;
            }

            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["RestoreFailed"]);
            throw;
        }
    }

    #endregion
}

