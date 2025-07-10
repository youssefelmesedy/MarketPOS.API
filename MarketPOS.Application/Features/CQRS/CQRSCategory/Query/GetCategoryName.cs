using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.CategoryDto;
using MediatR;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;

public record GetCategoryName : IRequest<ResultDto<IEnumerable<CategoryDetalisDto>>>
{
    public string? CategoryName { get; set; }

    public GetCategoryName(string? categoryName)
    {
        CategoryName = categoryName;
    }
}
