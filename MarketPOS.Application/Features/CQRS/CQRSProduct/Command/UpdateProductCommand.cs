namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Command;
public class UpdateProductCommand : IRequest<ResultDto<Guid>>
{
    public UpdateProductDto Dto { get; set; }
    public UpdateProductCommand(UpdateProductDto dto) => Dto = dto;
}
