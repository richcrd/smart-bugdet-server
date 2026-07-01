using SMB.APPLICATION.DTOs.Preferences;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IUserPreferenceService
{
    Task<UserPreferenceResponse> GetByUserId(long userId);
    Task<UserPreferenceResponse> Update(long userId, UpdateUserPreferenceRequest request);
}
