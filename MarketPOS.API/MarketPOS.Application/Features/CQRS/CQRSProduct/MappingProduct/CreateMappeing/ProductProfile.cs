namespace MarketPOS.Application.Features.CQRS.CQRSProduct.MappingProduct;
public partial class ProductProfile
{
    public void  MapProductCreate()
    {
        //Create Product
        CreateMap<CreateProductDto, Product>()
         .ForMember(dest => dest.Barcode,
             opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Barcode) ? "Not Found Barcode" : src.Barcode))
         .ForMember(dest => dest.ActiveIngredientId, opt => opt.MapFrom(src => src.IngredinentId))
         .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
         .ForMember(dest => dest.ProductPrice,
             opt => opt.MapFrom(src =>
                 new ProductPrice(
                     src.SalePrice,
                     src.DiscountPercentageFromSupplier > 0 ? src.DiscountPercentageFromSupplier : null,
                     src.PurchasePrice > 0 ? src.PurchasePrice : null
                 )
             ))
         .ForMember(dest => dest.ProductUnitProfile,
             opt => opt.MapFrom(src => new ProductUnitProfile
             {
                 LargeUnitName = src.LargeUnitName,
                 MediumUnitName = src.MediumUnitName,
                 SmallUnitName = src.SmallUnitName,
                 MediumPerLarge = src.MediumPerLarge,
                 SmallPerMedium = src.SmallPerMedium
             }))
         .AfterMap((src, dest) =>
         {
             if (dest.ProductUnitProfile != null && dest.ProductPrice != null)
             {
                 dest.ProductUnitProfile.LargeUnitPrice = dest.ProductPrice.SalePrice;

                 dest.ProductUnitProfile.CalculateUnitPricesFromLargeUnit();
             }
         })
         .ReverseMap();
    }
}
