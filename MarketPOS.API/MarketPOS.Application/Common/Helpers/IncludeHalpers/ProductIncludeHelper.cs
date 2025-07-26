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

                case ProductInclude.Product_InventorieAndWareHouse:
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
    public static List<IncludeExpressionOrFunc<Product>> GetSmartIncludes(List<ProductInclude> includes)
    {
        var result = new List<IncludeExpressionOrFunc<Product>>();

        foreach (var include in includes)
        {
            switch (include)
            {
                case ProductInclude.Category:
                    result.Add(new IncludeExpressionOrFunc<Product>
                    {
                        ExpressionInclude = p => p.Category!
                    });
                    break;

                case ProductInclude.Product_Price:
                    result.Add(new IncludeExpressionOrFunc<Product>
                    {
                        ExpressionInclude = p => p.ProductPrice
                    });
                    break;

                case ProductInclude.Product_UnitProfile:
                    result.Add(new IncludeExpressionOrFunc<Product>
                    {
                        ExpressionInclude = p => p.ProductUnitProfile
                    });
                    break;

                case ProductInclude.Product_Inventorie:
                    result.Add(new IncludeExpressionOrFunc<Product>
                    {
                        ExpressionInclude = p => p.ProductInventories
                    });
                    break;

                case ProductInclude.Product_InventorieAndWareHouse:
                    result.Add(new IncludeExpressionOrFunc<Product>
                    {
                        FuncInclude = q => q.Include(p => p.ProductInventories)
                                            .ThenInclude(pi => pi.Warehouse)
                    });
                    break;
            }
        }

        return result;
    }

}




