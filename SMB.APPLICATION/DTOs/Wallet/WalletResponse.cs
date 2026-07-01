namespace SMB.APPLICATION.DTOs.Wallet;

public class WalletResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long CurrencyId { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public string CurrencySymbol { get; set; } = null!;
    public decimal CurrentBalance { get; set; }
    public bool IsDefault { get; set; }
}
