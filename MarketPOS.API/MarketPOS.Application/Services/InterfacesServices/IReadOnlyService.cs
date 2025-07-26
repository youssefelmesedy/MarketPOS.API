using MarketPOS.Application.Services.InterfacesServices.GenericeInterface;

namespace MarketPOS.Application.Services.InterfacesServices;

public interface IReadOnlyService<T> : IQueryableService<T>, IProjectableService<T>
    where T : class
{ }
