namespace MarketPOS.Application.Features.CQRS.CQRSCategory.Query;
public record GetCategoryName : IRequest<ResultDto<IEnumerable<CategoryDetalisDto>>>
{
    public string? CategoryName { get; init; }
    public bool IncludSofteDelete { get; set; }
    public GetCategoryName(string? categoryName, bool includSofteDelete)
    {
        CategoryName = categoryName;
        IncludSofteDelete = includSofteDelete;
    }
}
