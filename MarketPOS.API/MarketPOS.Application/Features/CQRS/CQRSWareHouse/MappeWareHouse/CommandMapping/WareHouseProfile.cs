using Market.Domain.Entitys;
using MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;
using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.MappeWareHouse
{
    public partial class WareHouseProfile
    {
        public void WriteConfigureMappingsCreate()
        {
            CreateMap<WareHouseCreateDto, Warehouse>()
                 .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfoDto))
                 .ReverseMap();

            CreateMap<ContactInfoDto, ContactInfo>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();

            CreateMap<AddressInfoDto, AddressInfo>().ReverseMap();
        }
    }
}
