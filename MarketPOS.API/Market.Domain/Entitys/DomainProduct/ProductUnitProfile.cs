namespace Market.Domain.Entitys.DomainProduct;

// ProductUnitProfile.cs
public class ProductUnitProfile 
{
    public Guid ProductId { get; set; }

    public string LargeUnitName { get; set; } = default!;
    public string MediumUnitName { get; set; } = default!;
    public string SmallUnitName { get; set; } = default!;

    public int MediumPerLarge { get; set; } = 1;
    public int SmallPerMedium { get; set; } = 1;

    public decimal LargeUnitPrice { get; set; }
    public decimal MediumUnitPrice { get; set; }
    public decimal SmallUnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? ModifiedBy { get; set; }

    public Product Product { get; set; } = default!;

    public void CalculateUnitPricesFromLargeUnit()
    {
        if (MediumPerLarge <= 0 || SmallPerMedium <= 0)
            throw new InvalidOperationException("MediumPerLarge and SmallPerMedium must be greater than zero.");

        MediumUnitPrice = Math.Round(LargeUnitPrice / MediumPerLarge, 2);
        SmallUnitPrice = Math.Round(MediumUnitPrice / SmallPerMedium, 2);
    }
}

