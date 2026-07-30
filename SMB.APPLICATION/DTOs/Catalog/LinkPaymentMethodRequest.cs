namespace SMB.APPLICATION.DTOs.Catalog;

public class LinkPaymentMethodRequest
{
    public long PaymentMethodId { get; set; }
    public string? Alias { get; set; }
}