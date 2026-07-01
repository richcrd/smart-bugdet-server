using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IUserPreferenceRepository
{
    Task<UserPreference?> GetByUserId(long userId);
}
