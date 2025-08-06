using MarketPOS.Shared.DTOs.SofteDleteAndRestor;
namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.Query;
public class SofteDeleteWareHouseQuery : IRequest<ResultDto<SofteDeleteAndRestorDto>>
{
    public Guid Id { get; set; }

    public SofteDeleteWareHouseQuery(Guid id)
    {
        Id = id;
    }
}
