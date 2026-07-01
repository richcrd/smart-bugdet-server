using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class UserPreferenceRepository(AppDbContext dbContext) : IUserPreferenceRepository
{
    public async Task<UserPreference?> GetByUserId(long userId)
    {
        return await dbContext.UserPreferences
            .Include(x => x.DefaultCurrency)
            .Include(x => x.Language)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
