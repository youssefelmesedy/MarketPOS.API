namespace MarketPOS.Shared.Eunms.ProductEunms;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductInclude
{
    Category,
    Product_Price,
    Product_UnitProfile,
    Product_InventorieAndWareHouse,
    Product_Inventorie,
    Ingredinent
}

