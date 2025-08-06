using Market.Domain.Entitys.DomainCategory;
using System.Xml.Linq;

namespace Market.Domain.Entitys.DomainProduct;

// ProductUnitProfile.cs
public class  ProductUnitProfile : BaseEntity
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


    public Product Product { get; set; } = default!;

    public bool UpdateValues(string? largeUnitName, string? mediumUnitName, string? SmallUnitName, int? mediumPerLarge, int? smallPerMedium)
    {
        bool modified = false;

        if (!string.Equals(LargeUnitName?.Trim(), largeUnitName?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            LargeUnitName = largeUnitName?.Trim()!;
            modified = true;
        }

        if (!string.Equals(MediumUnitName?.Trim(), mediumUnitName?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            MediumUnitName = mediumUnitName?.Trim()!;
            modified = true;
        }

        if (!string.Equals(SmallUnitName?.Trim(), SmallUnitName?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            SmallUnitName = SmallUnitName?.Trim()!;
            modified = true;
        }

        if (MediumPerLarge != mediumPerLarge)
        {
            MediumPerLarge = mediumPerLarge!.Value;
            modified = true;
        }

        if (SmallPerMedium != smallPerMedium)
        {
            SmallPerMedium = smallPerMedium!.Value;
            modified = true;
        }

        if(modified)
        {
            UpdatedAt = DateTime.Now;
            ModifiedBy = "Youssef"; // Replace with actual user context if available
        }

        return modified;
    }

    public override void InitializeChildEntityinCreate()
    {
        CreatedAt = DateTime.Now;
        CreatedBy = "Youssef"; // Replace with actual user context if available
    }
    public void CalculateUnitPricesFromLargeUnit()
    {
        if (MediumPerLarge <= 0 || SmallPerMedium <= 0)
            throw new InvalidOperationException("MediumPerLarge and SmallPerMedium must be greater than zero.");

        MediumUnitPrice = Math.Round(LargeUnitPrice / MediumPerLarge, 2);
        SmallUnitPrice = Math.Round(MediumUnitPrice / SmallPerMedium, 2);
    }
}

