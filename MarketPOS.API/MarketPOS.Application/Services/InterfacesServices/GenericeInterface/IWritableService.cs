namespace MarketPOS.Application.Services.InterfacesServices.GenericeInterface;
public interface IWritableService<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task<TEntity> SoftDeleteAsync(TEntity entity);
    Task<Guid> RestoreAsync(Guid id);
}

