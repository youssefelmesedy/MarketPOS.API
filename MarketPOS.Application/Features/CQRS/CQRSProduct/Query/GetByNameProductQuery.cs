using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.ProductDto;
using MediatR;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;

public record GetByNameProductQuery : IRequest<ResultDto<IEnumerable<SomeFeaturesProductDto>>>
{
    public string? Name { get; set; }
    public GetByNameProductQuery(string name)
    {
        Name = name;
    }
}
