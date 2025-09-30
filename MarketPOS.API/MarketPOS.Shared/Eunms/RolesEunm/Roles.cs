namespace MarketPOS.Shared.Eunms.RolesEunm;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Roles
{
    Admin,
    User,
    Employee,
    Manager,
    Cashier,
    StockKeeper
}
