namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.MappingProfile;
public partial class ActiveIngredinentsProfile : Profile
{
    public ActiveIngredinentsProfile()
    {
        CommandIngredinentProfile();
        QueryIngredinent();
    }
}
