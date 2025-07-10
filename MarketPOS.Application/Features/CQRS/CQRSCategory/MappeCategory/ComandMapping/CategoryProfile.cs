namespace MarketPOS.Application.Features.CQRS.CQRSCategory.MappeCategory;
public partial class CategoryProfile
{
    public void MapCreateCategory()
    {
        CreateMap<CategoryCreateDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name!.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""));

    }
    public void MapUpdateCategory()
    {
        CreateMap<CategoryUpdateDto, Category>()
            .ForMember(dest => dest.Name, opt =>
            {
                opt.Condition(src => src.Name != null); // تجاهل null
                opt.MapFrom(src => src.Name!.Trim());
            })
            .ForMember(dest => dest.Description, opt =>
            {
                opt.Condition(src => src.Description != null); // تجاهل null
                opt.MapFrom(src => src.Description);
            })
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // عام لأي خاصية تانية
    }

}

