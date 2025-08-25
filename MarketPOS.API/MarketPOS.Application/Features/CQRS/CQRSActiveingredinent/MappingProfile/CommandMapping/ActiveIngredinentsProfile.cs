using MarketPOS.Shared.DTOs.ActivelngredientsDTO;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.MappingProfile;
public partial class ActiveIngredinentsProfile
{
    public void CommandIngredinentProfile()
    {
        CreateMap<ActiveIngredinents, CommandActiveIngredinentsDTO>()
             .ForMember(dest => dest.Name, opt =>
             {
                 opt.Condition(src => src.Name != null);
                 opt.MapFrom(src => src.Name!.Trim());
             }).ReverseMap();

        CreateMap<ActiveIngredinents, SofteDeleteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt));

        CreateMap<ActiveIngredinents, RestorDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.RestoredBy, opt => opt.MapFrom(src => src.RestorBy))
           .ForMember(dest => dest.RestoredAt, opt => opt.MapFrom(src => src.RestorAt));
    }
}
