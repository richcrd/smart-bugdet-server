using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.DTOs.Analytics;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Constants;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class AnalyticsRepository(AppDbContext dbContext) : IAnalyticsRepository
{
    public async Task<AnalyticsResponseDto> GetAnalyticsByUserId(long userId)
    {
        var today = AppTimeZone.TodayUtcMidnight;
        var startMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endMonth = startMonth.AddMonths(1);

        var defaultWallet = await dbContext.Wallets
            .Include(w => w.Currency)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.IsDefault);

        if (defaultWallet is null)
        {
            return new AnalyticsResponseDto
            {
                MonthlyTrend = GenerateEmptyTrend(startMonth)
            };
        }

        var monthTotals = await dbContext.Transactions
            .Where(t =>
                t.WalletId == defaultWallet.Id &&
                t.TransactionDate >= startMonth &&
                t.TransactionDate < endMonth)
            .GroupBy(t => t.TransactionType.Code)
            .Select(g => new { Code = g.Key, Total = g.Sum(t => t.Amount) })
            .ToListAsync();

        var trendStart = startMonth.AddMonths(-5);
        var trendEnd = startMonth.AddMonths(1);

        var monthlyGroups = await dbContext.Transactions
            .Where(t =>
                t.WalletId == defaultWallet.Id &&
                t.TransactionDate >= trendStart &&
                t.TransactionDate < trendEnd)
            .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Expenses = g.Where(t => t.TransactionType.Code == TransactionTypesCodes.Expense).Sum(t => t.Amount),
                Incomes = g.Where(t => t.TransactionType.Code == TransactionTypesCodes.Income).Sum(t => t.Amount)
            })
            .ToListAsync();

        var trend = new List<MonthlyTrendDto>();
        for (var i = 0; i < 6; i++)
        {
            var date = trendStart.AddMonths(i);
            var existing = monthlyGroups.FirstOrDefault(m => m.Year == date.Year && m.Month == date.Month);
            trend.Add(new MonthlyTrendDto
            {
                Year = date.Year,
                Month = date.Month,
                Expenses = existing?.Expenses ?? 0,
                Incomes = existing?.Incomes ?? 0
            });
        }

        return new AnalyticsResponseDto
        {
            CurrentBalance = defaultWallet.CurrentBalance,
            TotalIncomeMonth = monthTotals.FirstOrDefault(t => t.Code == TransactionTypesCodes.Income)?.Total ?? 0,
            TotalExpenseMonth = monthTotals.FirstOrDefault(t => t.Code == TransactionTypesCodes.Expense)?.Total ?? 0,
            CurrencyCode = defaultWallet.Currency.Code,
            CurrencySymbol = defaultWallet.Currency.Symbol,
            MonthlyTrend = trend
        };
    }

    private static List<MonthlyTrendDto> GenerateEmptyTrend(DateTime startMonth)
    {
        var trend = new List<MonthlyTrendDto>();
        for (var i = 0; i < 6; i++)
        {
            var date = startMonth.AddMonths(-5 + i);
            trend.Add(new MonthlyTrendDto { Year = date.Year, Month = date.Month });
        }
        return trend;
    }
}
