using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.DTOs.Transaction;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
{
    public async Task Add(Transaction transaction)
    {
        await dbContext.AddAsync(transaction);
    }

    public async Task<List<TransactionResponse>> GetByUserId(long userId)
    {
        return await dbContext.Transactions
            .Where(t => t.UserId == userId)
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
                CategoryIcon        = t.Category.Icon,
                CategoryColor       = t.Category.Color,
            })
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByUserAndDateRange(long userId, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
}