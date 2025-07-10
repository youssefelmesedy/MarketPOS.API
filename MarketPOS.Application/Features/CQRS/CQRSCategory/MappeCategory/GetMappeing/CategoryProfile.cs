using Market.Domain.Entitys.DomainCategory;
using MarketPOS.Shared.DTOs.CategoryDto;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.MappeCategory;

public partial class CategoryProfile
{
    public void MapCategoryGet()
    {
        // Category → CategoryDetalisDto
        CreateMap<Category, CategoryDetalisDto>()
            .ForMember(dest => dest.Id,
                       opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                       opt => opt.MapFrom(src => src.Name == null ? "Not Found Name Category" : src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy));
    }
}
