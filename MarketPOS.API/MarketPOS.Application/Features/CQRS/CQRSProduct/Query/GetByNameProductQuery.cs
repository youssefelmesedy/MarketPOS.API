namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Query;
public record GetByNameProductQuery : IRequest<ResultDto<IEnumerable<SomeFeaturesProductDto>>>
{
    public string? Name { get; init; }
    public bool IncludSofteDelete { get; init; }
    public GetByNameProductQuery(string name, bool includSofteDelete)
    {
        Name = name;
        IncludSofteDelete = includSofteDelete;
    }
}
