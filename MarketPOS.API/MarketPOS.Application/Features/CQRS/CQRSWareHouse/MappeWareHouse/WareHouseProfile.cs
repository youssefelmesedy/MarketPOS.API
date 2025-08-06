using AutoMapper;

namespace MarketPOS.Application.Features.CQRS.CQRSWareHouse.MappeWareHouse;
public partial class WareHouseProfile : Profile
{
    public WareHouseProfile()
    {
        WriteConfigureMappingsCreate();
        ReadConfigureMappings();
    }
}
