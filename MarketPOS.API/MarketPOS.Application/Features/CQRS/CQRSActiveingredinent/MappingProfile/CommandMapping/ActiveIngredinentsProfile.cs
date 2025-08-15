using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

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
             })
            .ForAllMembers(opt => opt.Condition((src, dest, srcMembers) => srcMembers != null));

        CreateMap<CommandActiveIngredinentsDTO, ActiveIngredinents>()
             .ForMember(dest => dest.Name, opt =>
             {
                 opt.Condition(src => src.Name != null);
                 opt.MapFrom(src => src.Name!.Trim());
             })
            .ForAllMembers(opt => opt.Condition((src, dest, srcMembers) => srcMembers != null));
    }
}
