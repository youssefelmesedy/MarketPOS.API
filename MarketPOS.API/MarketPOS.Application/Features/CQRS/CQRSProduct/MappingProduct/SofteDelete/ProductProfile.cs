using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.MappingProduct;
public partial class ProductProfile
{
    public void MapSofteDelete()
    {
        CreateMap<Product, SofteDeleteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt));

        CreateMap<ActiveIngredients, RestorDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.RestoredBy, opt => opt.MapFrom(src => src.RestorBy))
           .ForMember(dest => dest.RestoredAt, opt => opt.MapFrom(src => src.RestorAt));

    }
}
