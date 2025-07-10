namespace MarketPOS.Design.Discounts;

public interface IDiscountStrategy
{
    decimal ApplyDisCount(decimal orignalPrice);
}
