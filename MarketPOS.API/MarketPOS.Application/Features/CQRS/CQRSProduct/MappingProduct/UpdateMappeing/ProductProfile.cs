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
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}

