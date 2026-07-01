using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet);
    Task<Wallet?> GetById(long id);
    Task<Wallet?> GetDefaultByUserId(long id);
    Task<List<Wallet>> GetActiveByUserId(long userId);
    void Update(Wallet wallet);
}