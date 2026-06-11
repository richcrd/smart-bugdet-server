using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class UserSessionRepository(AppDbContext dbContext) : IUserSessionRepository
{
    public async Task<UserSession?> GetByRefreshToken(string hash)
    {
        return await dbContext.UserSessions
            .Include(x => x.User)
            .ThenInclude(x => x.Status)
            .FirstOrDefaultAsync(x => x.RefreshToken == hash);
    }

    public async Task Add(UserSession session)
    {
        await dbContext.UserSessions.AddAsync(session);
    }

    public async Task RevokeAllByUserId(long userId)
    {
        var sessions = await dbContext.UserSessions
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ToListAsync();

        foreach (var session in sessions)
        {
            session.RevokedAt = DateTime.UtcNow;
        }
    }
}