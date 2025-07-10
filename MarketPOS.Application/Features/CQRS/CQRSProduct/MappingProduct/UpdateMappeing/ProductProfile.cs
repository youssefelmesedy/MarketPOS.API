using Market.Domain.Entitys.DomainProduct;
using MarketPOS.Shared.DTOs.ProductDto;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.MappingProduct;

public partial class ProductProfile
{
    public void MapProductUpdate()
    {
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Name,
                opt =>
                {
                    opt.Condition(src => src.Name != null);
                    opt.MapFrom(src => src.Name);
                })
            .ForMember(dest => dest.Barcode,
                opt =>
                {
                    opt.Condition(src => src.Barcode != null);
                    opt.MapFrom(src => src.Barcode);
                })
            .ForMember(dest => dest.CategoryId,
                opt =>
                {
                    opt.Condition(src => src.CategoryId != Guid.Empty);
                    opt.MapFrom(src => src.CategoryId);
                })
            .AfterMap((src, dest) =>
            {
                // Just to be safe, skip mapping if the nested objects are null
                if (dest.ProductPrice == null || dest.ProductUnitProfile == null)
                    return;

                // ✅ Update Price
                if (dest.ProductPrice != null)
                {
                    dest.ProductPrice.SalePrice = src.SalePrice;

                    if (src.DiscountPercentageFromSupplier.HasValue && src.DiscountPercentageFromSupplier > 0)
                    {
                        dest.ProductPrice.DiscountPercentageFromSupplier = src.DiscountPercentageFromSupplier.Value;
                        dest.ProductPrice.PurchasePrice = ProductPrice.CalculatePurchasePriceFromDiscount(
                            src.SalePrice,
                            src.DiscountPercentageFromSupplier.Value
                        );
                    }
                    else if (src.PurchasePrice.HasValue && src.PurchasePrice > 0)
                    {
                        dest.ProductPrice.PurchasePrice = src.PurchasePrice.Value;
                        dest.ProductPrice.DiscountPercentageFromSupplier = ProductPrice.CalculateDiscountFromPurchase(
                            src.SalePrice,
                            src.PurchasePrice.Value
                        );
                    }
                }

                // ✅ Update UnitProfile
                if (dest.ProductUnitProfile != null)
                {
                    if (!string.IsNullOrWhiteSpace(src.LargeUnitName))
                        dest.ProductUnitProfile.LargeUnitName = src.LargeUnitName;

                    if (!string.IsNullOrWhiteSpace(src.MediumUnitName))
                        dest.ProductUnitProfile.MediumUnitName = src.MediumUnitName;

                    if (!string.IsNullOrWhiteSpace(src.SmallUnitName))
                        dest.ProductUnitProfile.SmallUnitName = src.SmallUnitName;

                    if (src.MediumPerLarge.HasValue)
                        dest.ProductUnitProfile.MediumPerLarge = src.MediumPerLarge.Value;

                    if (src.SmallPerMedium.HasValue)
                        dest.ProductUnitProfile.SmallPerMedium = src.SmallPerMedium.Value;

                    dest.ProductUnitProfile.LargeUnitPrice = src.SalePrice;

                    dest.ProductUnitProfile.CalculateUnitPricesFromLargeUnit();
                }
            })
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}

