namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query;
public class GetAllWareHouseQuery : IRequest<ResultDto<IEnumerable<WareHouseDetailsDto>>>
{
    public bool SofteDelete { get; set; }

    public GetAllWareHouseQuery(bool softeDelete)
    {
        SofteDelete = softeDelete;
    }
}
