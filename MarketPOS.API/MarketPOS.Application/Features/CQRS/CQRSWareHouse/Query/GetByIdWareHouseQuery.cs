using MarketPOS.Shared.DTOs.WareHouseDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query;
public class GetByIdWareHouseQuery : IRequest<ResultDto<WareHouseDetailsDto>>
{
    public Guid Id { get; set; }
    public bool SofteDelete { get; set; }

    public GetByIdWareHouseQuery(Guid id, bool softeDelete)
    {
        Id = id;
        SofteDelete = softeDelete;
    }
}
