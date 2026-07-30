using SMB.APPLICATION.DTOs.Expenses;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;

namespace SMB.APPLICATION.Services;

public class ExpensesService(
    ITransactionRepository transactionRepository,
    IWalletRepository walletRepository) : IExpensesService
{
    public async Task<ExpensesResponseDto> GetExpensesByMonth(long userId, int year, int month)
    {
        var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = from.AddMonths(1);

        var defaultWallet = await walletRepository.GetDefaultByUserId(userId);

        var transactions = await transactionRepository.GetByUserAndMonth(userId, from, to);

        return new ExpensesResponseDto
        {
            TotalExpenses = transactions.Sum(t => t.Amount),
            CurrencySymbol = defaultWallet?.Currency?.Symbol ?? "C$",
            Transactions = transactions,
        };
    }
}
