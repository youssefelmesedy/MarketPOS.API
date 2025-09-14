using Market.Domain.Entitys;

namespace MarketPOS.Application.Specifications.Products;

public class GetProductWithIncludesSpecification : BaseSpecification<Product>
{
    public GetProductWithIncludesSpecification(
        Guid? categoryId,
        List<ProductInclude> includes,
        bool includeSoftDelete,
        int pageSize = 0,
        int pageIndex = 1) // نبدأ من 1
    {
        Criteria = categoryId.HasValue
            ? p => p.CategoryId == categoryId.Value
            : p => true;

        var smartIncludes = ProductIncludeHelper.GetSmartIncludes(includes);

        foreach (var include in smartIncludes)
        {
            if (include.ISFunc)
                AddInclude(include.FuncInclude!);
            else
                AddInclude(include.ExpressionInclude!);
        }

        ApplyOrderBy(p => p.OrderBy(c => c.Id));

        // تحويل PageIndex بحيث يبدأ من 1
        var skip = (pageIndex > 0 ? pageIndex - 1 : 0) * pageSize;
        ApplyPaging(skip, pageSize);

        EnableSoftDeleted(includeSoftDelete);
    }
}


