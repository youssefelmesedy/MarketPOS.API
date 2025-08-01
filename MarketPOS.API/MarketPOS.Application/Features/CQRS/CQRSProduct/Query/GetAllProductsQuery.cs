﻿namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
public record GetAllProductsQuery : IRequest<ResultDto<List<SomeFeaturesProductDto>>>
{
    public bool SofteDelete { get; set; }

    public GetAllProductsQuery(bool softeDelete)
    {
        SofteDelete = softeDelete;
    }
}
    