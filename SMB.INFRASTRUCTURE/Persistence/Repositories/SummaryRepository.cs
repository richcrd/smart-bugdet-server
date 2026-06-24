using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.DTOs.Summary;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Constants;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class SummaryRepository(AppDbContext dbContext) : ISummaryRepository
{
    public async Task<DashboardSummaryDto> GetSummaryByUserId(long userId)
    {
        var startMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endMonth = startMonth.AddMonths(1);
        
        var balance = await dbContext.Wallets
            .Where(w => w.UserId == userId && w.IsDefault)
            .SumAsync(w => w.CurrentBalance);
        
        var transactionTotals = await dbContext.Transactions
            .Where(t =>
                t.UserId == userId &&
                t.TransactionDate >= startMonth &&
                t.TransactionDate < endMonth)
            .GroupBy(t => t.TransactionType.Code)
            .Select(g => new { Code = g.Key, Total = g.Sum(t => t.Amount) })
            .ToListAsync();

        return new DashboardSummaryDto
        {
            CurrentBalance = balance,
            TotalIncomeMonth = transactionTotals.FirstOrDefault(t => t.Code == TransactionTypesCodes.Income)?.Total  ?? 0,
            TotalExpenseMonth = transactionTotals.FirstOrDefault(t => t.Code == TransactionTypesCodes.Expense)?.Total ?? 0,
        };
    }
}