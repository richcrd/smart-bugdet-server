namespace SMB.APPLICATION.DTOs.Catalog;

public class UserPaymentMethodResponse
{
    public long Id { get; set; }
    public long PaymentMethodId { get; set; }
    public string Name { get; set; } = null!;
    public string? Alias { get; set; }
}