namespace SupportDesk.Data.Entities;

public class Address
{
    public int AddressId { get; set; }
    public int CustomerId { get; set; }
    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    public Customer Customer { get; set; } = null!;
}