using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class UserExternalLoginRepository(AppDbContext dbContext) : IUserExternalLoginRepository
{
    public async Task<UserExternalLogin?> GetByProviderAndUserId(string provider, string providerUserId)
    {
        return await dbContext.UserExternalLogins
            .Include(x => x.User)
                .ThenInclude(u => u.Status)
            .FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderUserId == providerUserId);
    }

    public async Task Add(UserExternalLogin externalLogin)
    {
        await dbContext.UserExternalLogins.AddAsync(externalLogin);
    }
}
