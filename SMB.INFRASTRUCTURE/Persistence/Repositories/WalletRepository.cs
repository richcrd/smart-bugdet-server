using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class WalletRepository(AppDbContext dbContext) : IWalletRepository
{
    public async Task AddAsync(Wallet wallet)
    {
        await dbContext.Wallets.AddAsync(wallet);
    }

    public async Task<Wallet?> GetById(long id)
    {
        return await dbContext.Wallets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Wallet?> GetDefaultByUserId(long id)
    {
        return await dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == id && x.IsDefault);
    }

    public void Update(Wallet wallet)
    {
        dbContext.Wallets.Update(wallet);
    }
}