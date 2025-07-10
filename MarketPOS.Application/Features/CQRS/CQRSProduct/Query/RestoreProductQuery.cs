using MarketPOS.Shared.DTOs;
using MediatR;

namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;

public record RestoreProductQuery : IRequest<ResultDto<Guid>>
{
    public Guid Id { get; init; } // The ID of the product to be restored
    public RestoreProductQuery(Guid id) => Id = id; // Constructor to initialize the Id property
}
