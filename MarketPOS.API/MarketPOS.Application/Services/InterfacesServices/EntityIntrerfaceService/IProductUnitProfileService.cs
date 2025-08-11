namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductUnitProfileService : IFullService<ProductUnitProfile>
{
    Task<ProductUnitProfile> GetByProductIdAsync(Guid productId);
    Task<int> UpdateByProductIdAsync(ProductUnitProfile productUnitProfile);
}
