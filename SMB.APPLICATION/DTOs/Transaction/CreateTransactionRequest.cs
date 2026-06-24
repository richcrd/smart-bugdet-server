namespace SMB.APPLICATION.DTOs.Transaction;

public class CreateTransactionRequest
{
    public long WalletId { get; set; }
    public long TransactionTypeId { get; set; }
    public long CategoryId { get; set; }
    public long? SubcategoryId { get; set; }
    public long? PaymentMethodId { get; set; }
    public long CurrencyId { get; set; }

    public decimal Amount { get; set; }
    public decimal? ExchangeRate { get; set; }

    public string? Description { get; set; }
    public DateOnly TransactionDate { get; set; }
}
