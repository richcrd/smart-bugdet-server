using SMB.APPLICATION.DTOs.Transaction;

namespace SMB.APPLICATION.Interfaces.Services;

public interface ITransactionService
{
    Task<long> Create(CreateTransactionRequest request, long userId);
    Task<List<TransactionResponse>> GetAll(long userId);
}