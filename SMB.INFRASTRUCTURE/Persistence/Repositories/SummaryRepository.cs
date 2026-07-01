using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.DTOs.Summary;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Constants;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class SummaryRepository(AppDbContext dbContext) : ISummaryRepository
{
    public async Task<DashboardSummaryDto> GetSummaryByUserId(long userId)
    {
        var today = AppTimeZone.TodayUtcMidnight;
        var startMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endMonth = startMonth.AddMonths(1);
        
        var defaultWallet = await dbContext.Wallets
            .Include(w => w.Currency)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.IsDefault);

        if (defaultWallet is null)
        {
            return new DashboardSummaryDto();
        }

        var transactionTotals = await dbContext.Transactions
            .Where(t =>
                t.WalletId == defaultWallet.Id &&
                t.TransactionDate >= startMonth &&
                t.TransactionDate < endMonth)
            .GroupBy(t => t.TransactionType.Code)
            .Select(g => new { Code = g.Key, Total = g.Sum(t => t.Amount) })
            .ToListAsync();

        return new DashboardSummaryDto
        {
            CurrentBalance = defaultWallet.CurrentBalance,
            TotalIncomeMonth = transactionTotals.FirstOrDefault(t => t.Code == TransactionTypesCodes.Income)?.Total ?? 0,
            TotalExpenseMonth = transactionTotals.FirstOrDefault(t => t.Code == TransactionTypesCodes.Expense)?.Total ?? 0,
            CurrencyCode = defaultWallet.Currency.Code,
            CurrencySymbol = defaultWallet.Currency.Symbol,
        };
    }
}