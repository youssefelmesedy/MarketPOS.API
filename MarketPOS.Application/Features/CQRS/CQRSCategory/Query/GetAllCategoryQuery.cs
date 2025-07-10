using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.CategoryDto;
using MediatR;

namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;

public class GetAllCategoryQuery : IRequest<ResultDto<IEnumerable<CategoryDetalisDto>>>
{
    public bool SoftDelete { get; set; }

    public GetAllCategoryQuery(bool softDelete)
    {
        SoftDelete = softDelete;
    }
}
