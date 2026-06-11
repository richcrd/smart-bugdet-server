using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IUserSessionRepository
{
    Task<UserSession?> GetByRefreshToken(string hash);
    Task Add(UserSession session);
    Task RevokeAllByUserId(long userId);
}