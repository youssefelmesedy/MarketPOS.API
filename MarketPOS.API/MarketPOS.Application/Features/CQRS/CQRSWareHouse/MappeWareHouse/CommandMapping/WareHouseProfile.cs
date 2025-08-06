namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.MappeWareHouse
{
    public partial class WareHouseProfile
    {
        public void WriteConfigureMappings()
        {
            CreateMap<WareHouseCreateDto, Warehouse>()
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfoDto));
            CreateMap<WareHouseUpdateDto, Warehouse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfoDto));
            CreateMap<Warehouse, WareHouseDetailsDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContactInfoDto, opt => opt.MapFrom(src => src.ContactInfo));
        }
    }
}
