namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductPriceService :IFullService<ProductPrice>, IReadOnlyService<ProductPrice>
{
    Task<ProductPrice> GetByProductIdAsync(Guid productId);
    Task<int> UpdateByProductIdAsync(ProductPrice productPrice);
}
