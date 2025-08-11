using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.MappingProfile;
public partial class ActiveIngredinentsProfile
{
    public void CommandWareHouseProfile()
    {
        CreateMap<Market.Domain.Entitys.DomainCategory.ActiveIngredinents, ActiveIngredinentsCreateDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ReverseMap();
    }
}
