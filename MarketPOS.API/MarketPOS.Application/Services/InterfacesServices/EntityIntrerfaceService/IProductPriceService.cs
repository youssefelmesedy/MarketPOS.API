namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductPriceService : IFullService<ProductPrice>
{
    Task<ProductPrice?> GetByProductIdAsync(Guid productId);
    Task<int> UpdateByProductIdAsync(ProductPrice productPrice);
}
