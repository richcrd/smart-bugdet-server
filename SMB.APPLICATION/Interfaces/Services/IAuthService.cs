using SMB.APPLICATION.DTOs.Auth;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IAuthService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request);
    Task<LoginResponse> Login(LoginRequest request);
    Task<LoginResponse> Refresh(RefreshTokenRequest request);
    Task Logout(string refreshToken, long userId);
    Task LogoutAll(long userId);
}