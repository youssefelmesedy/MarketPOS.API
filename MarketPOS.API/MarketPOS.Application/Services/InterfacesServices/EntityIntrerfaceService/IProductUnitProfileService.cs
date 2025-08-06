namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductUnitProfileService : IFullService<ProductUnitProfile>, IReadOnlyService<ProductUnitProfile>
{
    Task<ProductUnitProfile> GetByProductIdAsync(Guid productId);
    Task<int> UpdateByProductIdAsync(ProductUnitProfile productUnitProfile);
}
