using SMB.APPLICATION.DTOs.Auth;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IAuthService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request);
}