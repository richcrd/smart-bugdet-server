using SMB.APPLICATION.DTOs.User;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IUserService
{
    Task UpdateProfile(long userId, UpdateProfileRequest request);
    Task ChangePassword(long userId, ChangePasswordRequest request);
    Task<UserProfileResponse> GetProfile(long userId);
}
