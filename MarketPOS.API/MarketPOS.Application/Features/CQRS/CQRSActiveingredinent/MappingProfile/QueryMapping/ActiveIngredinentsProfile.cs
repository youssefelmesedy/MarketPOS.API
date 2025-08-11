using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.MappingProfile;
public partial class ActiveIngredinentsProfile
{
    public void QueryActiveIngredinent()
    {
        CreateMap<ActiveIngredinents, ActiveIngredinentsDetalisDTO>()
            .ForMember(dets => dets.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dets => dets.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dets => dets.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dets => dets.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dets => dets.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dets => dets.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dets => dets.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
            .ForMember(dets => dets.DeleteBy, opt => opt.MapFrom(src => src.DeleteBy))
            .ForMember(dets => dets.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dets => dets.RestoreAt, opt => opt.MapFrom(src => src.RestorAt))
            .ForMember(dets => dets.RestoreBy, opt => opt.MapFrom(src => src.RestorBy));
    }
}
