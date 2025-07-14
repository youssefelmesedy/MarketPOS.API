namespace MarketPOS.Application.Features.CQRS.CQRSProduct.MappingProduct;
public partial class ProductProfile : Profile
{
    public ProductProfile()
    {
        MapProductPagenation();
        MapProductGetAllAndGetById();
        MapProductCreate();
        MapProductUpdate();
        MapSofteDelete();
    }
}

