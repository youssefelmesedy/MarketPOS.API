using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query;
public class RestorWareHouseQuery  : IRequest<ResultDto<SofteDeleteAndRestorDto>>
{
    public Guid Id { get; set; }

    public RestorWareHouseQuery(Guid id)
    {
        Id = id;
    }
}
