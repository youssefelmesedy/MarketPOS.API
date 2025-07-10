

namespace MarketPOS.Application.Common.Helpers.IncludeHalpers;

public static class ProductIncludeHelper
{
    public static List<Func<IQueryable<Product>, IQueryable<Product>>> GetIncludeExpressions(List<ProductInclude>? includes)
    {
        var expressions = new List<Func<IQueryable<Product>, IQueryable<Product>>>();

        if (includes is null || !includes.Any())
            return expressions;

        foreach (var include in includes)
        {
            switch (include)
            {
                case ProductInclude.Category:
                    expressions.Add(q => q.Include(p => p.Category));
                    break;
                case ProductInclude.Product_Inventorie:
                    expressions.Add(q => q.Include(p => p.ProductInventories)
                                          .ThenInclude(pi => pi.Warehouse));
                    break;
                case ProductInclude.Product_Price:
                    expressions.Add(q => q.Include(p => p.ProductPrice));
                    break;
                case ProductInclude.Product_UnitProfile:
                    expressions.Add(q => q.Include(p => p.ProductUnitProfile));
                    break;
            }
        }

        return expressions;
    }

}



