namespace Market.Domain.Entitys;
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }

    public string DeleteBy { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    public DateTime? RestorAt { get; set; }
    public String? RestorBy { get; set; }

    public virtual void InitializeChildEntityinCreate()
    {
        CreatedAt = DateTime.Now;
        CreatedBy = "Youssef";
        IsDeleted = false;
    }

    public void InitializeChildEntityinUpdate()
    {
        UpdatedAt = DateTime.Now;
        ModifiedBy = "Youssef";
    }
}
public class ContactInfo
{
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public AddressInfo? Address { get; set; }
}
public class AddressInfo
{
    public string Country { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Street { get; set; } 
}


