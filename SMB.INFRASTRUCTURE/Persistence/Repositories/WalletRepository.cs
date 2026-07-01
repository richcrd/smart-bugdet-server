using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Constants;
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
        return await dbContext.Wallets.Include(x => x.Currency).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Wallet?> GetDefaultByUserId(long id)
    {
        return await dbContext.Wallets.FirstOrDefaultAsync(x => x.UserId == id && x.IsDefault);
    }

    public async Task<List<Wallet>> GetActiveByUserId(long userId)
    {
        return await dbContext.Wallets
            .Include(x => x.Currency)
            .Where(x => x.UserId == userId && x.Status.Code == StatusCodes.Active)
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public void Update(Wallet wallet)
    {
        dbContext.Wallets.Update(wallet);
    }
}