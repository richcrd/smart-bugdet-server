using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IWalletRepository
{
    Task AddAsync(Wallet wallet);
    Task<Wallet?> GetById(long id);
    Task<Wallet?> GetDefaultByUserId(long id);
    void Update(Wallet wallet);
}