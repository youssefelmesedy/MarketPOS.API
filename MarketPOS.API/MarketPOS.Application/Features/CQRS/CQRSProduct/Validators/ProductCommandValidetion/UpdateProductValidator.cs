namespace MarketPOS.Application.Features.CQRS.CQRSProduct.Validators.ProductValidetion;
public class UpdateProductValidator : BaseValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleForPrice(x => x.Dto.SalePrice, "Sale Price");

        RuleForOneRequired(
            x => x.Dto.PurchasePrice,
            x => x.Dto.DiscountPercentageFromSupplier,
            "Either purchase price or supplier discount must be provided."
        );

        RuleForNullablePercentage(x => x.Dto.DiscountPercentageFromSupplier, "Supplier Discount");
        RuleForExpirationDate(x => x.Dto.ExpirationDate, "ExpirationDate Not valide");


        When(x => x.Dto.MediumPerLarge != null, () =>
        {
            RuleForUnitConversion(x => x.Dto.MediumPerLarge!.Value, "Medium per Large");
        });

        When(x => x.Dto.SmallPerMedium != null, () =>
        {
            RuleForUnitConversion(x => x.Dto.SmallPerMedium!.Value, "Small per Medium");
        });

        When(x => x.Dto.DiscountPercentageFromSupplier == 0, () =>
        {
            RuleFor(x => x.Dto.PurchasePrice)
                .NotNull().WithMessage("Purchase price is required when discount is 0.")
                .LessThan(x => x.Dto.SalePrice)
                .WithMessage("Purchase price must be less than sale price.");
        });
    }
}

