using Market.Domain.Entitys;

namespace MarketPOS.Application.Specifications.Products;

public class GetProductWithIncludesSpecification  : BaseSpecification<Product>
{
    public GetProductWithIncludesSpecification (Guid? categoryId, List<ProductInclude> includes, bool IncludeSofteDelete, int pageSize = 0, int pageIndex = 0)
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

        ApplyPaging(pageSize, pageIndex);

        EnableSoftDeleted(IncludeSoftDeleted);
    }
}

