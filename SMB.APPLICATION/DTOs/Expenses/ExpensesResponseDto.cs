using SMB.APPLICATION.DTOs.Transaction;

namespace SMB.APPLICATION.DTOs.Expenses;

public class ExpensesResponseDto
{
    public decimal TotalExpenses { get; set; }
    public string CurrencySymbol { get; set; } = "";
    public List<TransactionResponse> Transactions { get; set; } = [];
}
