namespace SMB.APPLICATION.DTOs.Wallet;

public class CreateWalletRequest
{
    public string Name { get; set; } = null!;
    public long CurrencyId { get; set; }
}
