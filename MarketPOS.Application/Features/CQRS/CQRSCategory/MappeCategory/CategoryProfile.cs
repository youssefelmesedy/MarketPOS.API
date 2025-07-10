using AutoMapper;
using Microsoft.Extensions.Localization;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.MappeCategory;

public partial class CategoryProfile : Profile
{
    public CategoryProfile()
    {

        MapCategoryGet();
        MapCreateCategory();
        MapUpdateCategory();
    }
}
