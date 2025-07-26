namespace MarketPOS.Application.Common.Helpers.IncludeHalpers;

public class IncludeExpressionOrFunc<T>
{
    public Expression<Func<Product, object>>? ExpressionInclude { get; set; }
    public Func<IQueryable<Product>, IQueryable<Product>>? FuncInclude { get; set; }

    public bool ISFunc => FuncInclude is not null;
}
