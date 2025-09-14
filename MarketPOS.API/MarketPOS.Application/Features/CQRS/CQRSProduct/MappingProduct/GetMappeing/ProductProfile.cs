namespace MarketPOS.Application.Features.CQRS.CQRSProduct.MappingProduct;
public partial class ProductProfile
{
    public void MapProductGetAllAndGetById()
    {
        // Product → SomeFeaturesProductDto
        CreateMap<Product, SomeFeaturesProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Not Found Category Name"))

            .ForMember(dest => dest.IngredinentId,
                opt => opt.MapFrom(src => src.ProductIngredients != null
                  ? src.ProductIngredients.Select(pi => pi.ActiveIngredinentsId).ToList()
                  : new List<Guid>()))

            .ForMember(dest => dest.IngredinentName,
                opt => opt.MapFrom(src => src.ProductIngredients != null 
                  ? src.ProductIngredients.Select(pi => pi.ActiveIngredinents.Name).ToList() 
                  : new List<string?>() { "Not Found Ingredients "}))

            .ForMember(dest => dest.WaerHousId,
                opt => opt.MapFrom(src => src.ProductInventories != null
                    ? string.Join(", ", src.ProductInventories
                        .Where(pi => pi.Warehouse != null)
                        .Select(pi => pi.Warehouse.Id.ToString()))
                    : "No Warehouses Found"))

            .ForMember(dest => dest.WaerHousName,
                opt => opt.MapFrom(src => src.ProductInventories != null
                    ? string.Join(", ", src.ProductInventories
                        .Where(pi => pi.Warehouse != null)
                        .Select(pi => pi.Warehouse.Name))
                    : "No Warehouses Found"))

            .ForMember(dest => dest.Barcode,
                opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Barcode) ? "Not Found" : src.Barcode))

            .ForMember(dest => dest.PurchasePrice,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.PurchasePrice : 0))

            .ForMember(dest => dest.SalePrice,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.SalePrice : 0))

            .ForMember(dest => dest.DiscountPercentageFromSupplier,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.DiscountPercentageFromSupplier : 0))
            .ReverseMap();
    }
}
