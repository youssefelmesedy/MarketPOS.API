using MarketPOS.Application.Services.InterfacesServices.GenericeInterface;

namespace MarketPOS.Application.Services.InterfacesServices;

public interface IFullService<T> : IReadOnlyService<T>, IWritableService<T>
    where T : class
{ }
