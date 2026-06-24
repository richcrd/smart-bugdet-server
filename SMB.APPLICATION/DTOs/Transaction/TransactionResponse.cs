namespace SMB.APPLICATION.DTOs.Transaction;

public class TransactionResponse
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
    public DateOnly TransactionDate { get; set; }
    public string TransactionTypeCode { get; set; } = "";
    public string TransactionTypeName { get; set; } = "";
}