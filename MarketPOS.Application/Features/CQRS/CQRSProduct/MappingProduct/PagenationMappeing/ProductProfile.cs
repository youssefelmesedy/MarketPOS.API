using Market.Domain.Entitys.DomainProduct;
using MarketPOS.Shared.DTOs.ProductDto;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.MappingProduct;

public partial class ProductProfile
{
    public void MapProductPagenation()
    {
        // Product → ProductDetailsDto
        CreateMap<Product, ProductDetailsDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
            .ForMember(dest => dest.Barcode,
                opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Barcode) ? "Not Found Barcode" : src.Barcode))
            .ForMember(dest => dest.PurchasePrice,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.PurchasePrice : 0))
            .ForMember(dest => dest.SalePrice,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.SalePrice : 0))
            .ForMember(dest => dest.DiscountPercentageFromSupplier,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.DiscountPercentageFromSupplier : 0))
            .ForMember(dest => dest.LargeUnitName,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.LargeUnitName : ""))
            .ForMember(dest => dest.MediumUnitName,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.MediumUnitName : ""))
            .ForMember(dest => dest.SmallUnitName,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.SmallUnitName : ""))
            .ForMember(dest => dest.MediumPerLarge,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.MediumPerLarge : 0))
            .ForMember(dest => dest.SmallPerMedium,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.SmallPerMedium : 0))
            .ForMember(dest => dest.LargeUnitPrice,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.LargeUnitPrice : 0))
            .ForMember(dest => dest.MediumUnitPrice,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.MediumUnitPrice : 0))
            .ForMember(dest => dest.SmallUnitPrice,
                opt => opt.MapFrom(src => src.ProductUnitProfile != null ? src.ProductUnitProfile.SmallUnitPrice : 0))
            .ForMember(dest => dest.TotalQuantityInStock,
                opt => opt.MapFrom(src => src.ProductInventories != null ? src.ProductInventories.Sum(pi => pi.Quantity) : 0))
            .ForMember(dest => dest.WarehouseName,
                opt => opt.MapFrom(src => src.ProductInventories != null
                    ? string.Join(", ", src.ProductInventories
                        .Where(pi => pi.Warehouse != null)
                        .Select(pi => pi.Warehouse.Name))
                    : "No Warehouses Found"))
            .ReverseMap();
    }
}
