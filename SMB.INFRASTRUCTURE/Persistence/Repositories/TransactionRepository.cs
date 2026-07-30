using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.DTOs.Transaction;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
{
    public async Task Add(Transaction transaction)
    {
        await dbContext.AddAsync(transaction);
    }

    public async Task<List<TransactionResponse>> GetByWalletId(long walletId)
    {
        var today = AppTimeZone.TodayUtcMidnight;
        var tomorrow = today.AddDays(1);

        return await dbContext.Transactions
            .Where(t =>
                t.WalletId == walletId &&
                t.TransactionDate >= today &&
                t.TransactionDate < tomorrow)
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => new TransactionResponse
            {
                Id                  = t.Id,
                Amount              = t.Amount,
                Description         = t.Description ?? "",
                TransactionDate     = DateOnly.FromDateTime(t.TransactionDate),
                TransactionTypeCode = t.TransactionType.Code,
                TransactionTypeName = t.TransactionType.Name,
                CategoryName        = t.Category.Name,
                SubcategoryName     = t.Subcategory.Name,
                CategoryIcon        = t.Category.Icon,
                CategoryColor       = t.Category.Color,
                CurrencyCode        = t.Currency.Code,
                CurrencySymbol      = t.Currency.Symbol,
            })
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByUserAndDateRange(long userId, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TransactionResponse>> GetByUserAndMonth(long userId, DateTime from, DateTime to)
    {
        return await dbContext.Transactions
            .Where(t =>
                t.UserId == userId &&
                t.TransactionDate >= from &&
                t.TransactionDate < to &&
                t.TransactionType.Code == TransactionTypesCodes.Expense)
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => new TransactionResponse
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description ?? "",
                TransactionDate = DateOnly.FromDateTime(t.TransactionDate),
                TransactionTypeCode = t.TransactionType.Code,
                TransactionTypeName = t.TransactionType.Name,
                CategoryName = t.Category.Name,
                SubcategoryName = t.Subcategory != null ? t.Subcategory.Name : null,
                CategoryIcon = t.Category.Icon,
                CategoryColor = t.Category.Color,
                CurrencyCode = t.Currency.Code,
                CurrencySymbol = t.Currency.Symbol,
            })
            .ToListAsync();
    }

    public async Task<bool> ExistsForUserOnDate(long userId, DateTime date)
    {
        return await dbContext.Transactions.AnyAsync(t => t.UserId == userId && t.TransactionDate == date);
    }
}