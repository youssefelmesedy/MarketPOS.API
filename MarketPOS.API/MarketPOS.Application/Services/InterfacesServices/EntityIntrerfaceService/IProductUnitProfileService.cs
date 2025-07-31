using MarketPOS.Application.Services.InterfacesServices.GenericeInterface;

namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductUnitProfileService : IProjectableService<ProductUnitProfile>,
    IQueryableService<ProductUnitProfile>,
    IWritableService<ProductUnitProfile>
{
    Task<ProductUnitProfile> GetByProductIdAsync(Guid productId);
    Task<int> UpdateByProductIdAsync(ProductUnitProfile productUnitProfile);
}
