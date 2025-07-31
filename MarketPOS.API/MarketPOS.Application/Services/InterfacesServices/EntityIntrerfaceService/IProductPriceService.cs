using MarketPOS.Application.Services.InterfacesServices.GenericeInterface;

namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductPriceService :IProjectableService<ProductPrice>,
    IQueryableService<ProductPrice>,
    IWritableService<ProductPrice>
{
    Task<ProductPrice> GetByProductIdAsync(Guid productId);
    Task<int> UpdateByProductIdAsync(ProductPrice productPrice);
}
