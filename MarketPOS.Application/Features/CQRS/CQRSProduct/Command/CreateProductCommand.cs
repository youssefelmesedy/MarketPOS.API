using MarketPOS.Shared.DTOs;
using MarketPOS.Shared.DTOs.ProductDto;
using MediatR;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command;

public class CreateProductCommand : IRequest<ResultDto<Guid>>
{
    public CreateProductDto Dto { get; }

    public CreateProductCommand(CreateProductDto dto)
    {
        Dto = dto;
    }
}

