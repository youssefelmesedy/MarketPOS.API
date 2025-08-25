using MarketPOS.Shared.DTOs.SofteDleteAndRestor;
using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.MappeWareHouse;
public partial class WareHouseProfile
{
    public void ReadConfigureMappings()
    {
        CreateMap<Warehouse, WareHouseDetailsDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContactInfoDto, opt => opt.MapFrom(src => src.ContactInfo)).ReverseMap();

        CreateMap<Warehouse, RestorDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.RestoredBy, opt => opt.MapFrom(src => src.RestorBy))
            .ForMember(dest => dest.RestoredAt, opt => opt.MapFrom(src => src.RestorAt));

        CreateMap<Warehouse, SofteDeleteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt));
    }
}
