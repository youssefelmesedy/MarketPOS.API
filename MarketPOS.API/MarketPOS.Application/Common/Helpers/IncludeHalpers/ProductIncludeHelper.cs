namespace MarketPOS.Application.Common.Helpers.IncludeHalpers;
public static class ProductIncludeHelper
{
    /// <summary>
    /// Generates a list of LINQ expressions to include related entities in a query for <see cref="Product"/> objects.
    /// </summary>
    /// <remarks>This method is typically used to dynamically construct query expressions for including
    /// related entities  in a database query. The supported related entities are determined by the values in the <see
    /// cref="ProductInclude"/> enumeration.</remarks>
    /// <param name="includes">A list of <see cref="ProductInclude"/> values specifying the related entities to include.  If <paramref
    /// name="includes"/> is <see langword="null"/> or empty, an empty list is returned.</param>
    /// <returns>A list of functions that, when applied to an <see cref="IQueryable{T}"/> of <see cref="Product"/>,  include the
    /// specified related entities. The list will be empty if no includes are specified.</returns>
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

                case ProductInclude.Ingredinent:
                    expressions.Add(q => q.Include(p => p.ProductIngredients)
                                   .ThenInclude(p => p.ActiveIngredinents));
                    break;
            }
        }

        return expressions;
    }

    /// <summary>
    /// Generates a list of include expressions or functions for the specified product-related includes.
    /// </summary>
    /// <remarks>This method maps each <see cref="ProductInclude"/> value to its corresponding include
    /// expression or function. The resulting list can be used to configure data retrieval operations, such as Entity
    /// Framework queries, to include related entities.</remarks>
    /// <param name="includes">A list of <see cref="ProductInclude"/> values that specify the product-related entities to include.</param>
    /// <returns>A list of <see cref="IncludeExpressionOrFunc{T}"/> objects representing the include expressions or functions for
    /// the specified product-related entities.</returns>
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
                case ProductInclude.Ingredinent:
                    result.Add(new IncludeExpressionOrFunc<Product>
                    {
                        FuncInclude = q => q.Include(p => p.ProductIngredients)
                                            .ThenInclude(pi => pi.ActiveIngredinents)
                    });
                    break;
            }
        }

        return result;
    }

}




