using SMB.APPLICATION.DTOs.Transaction;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task Add(Transaction transaction);
    Task<List<TransactionResponse>> GetByUserId(long userId);
    Task<List<Transaction>> GetByUserAndDateRange(long userId, DateTime from, DateTime to);
}