namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Validators.ProductValidetion;
public class CreateProductValidator : BaseValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleForName(x => x.Dto.Name);
        RuleForCategoryId(x => x.Dto.CategoryId, "CategoryId");
        RuleForPrice(x => x.Dto.SalePrice, "Sale Price");

        RuleForOneRequired(
            x => x.Dto.PurchasePrice,
            x => x.Dto.DiscountPercentageFromSupplier,
            "Either purchase price or supplier discount must be provided."
        );

        RuleForNullablePercentage(x => x.Dto.DiscountPercentageFromSupplier, "Supplier Discount");
        RuleForExpirationDate(x => x.Dto.ExpirationDate, "ExpirationDate");

        RuleForUnitConversion(x => x.Dto.MediumPerLarge, "Medium per Large");
        RuleForUnitConversion(x => x.Dto.SmallPerMedium, "Small per Medium");

        When(x => x.Dto.DiscountPercentageFromSupplier == 0, () =>
        {
            RuleFor(x => x.Dto.PurchasePrice)
                .NotNull().WithMessage("Purchase price is required when discount is 0.")
                .LessThan(x => x.Dto.SalePrice)
                .WithMessage("Purchase price must be less than sale price.");
        });
    }
}




