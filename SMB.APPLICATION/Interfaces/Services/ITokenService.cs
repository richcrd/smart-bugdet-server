using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Services;

public interface ITokenService
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
    string HashRefreshToken(string refreshToken);
    DateTime GetRefreshTokenExpiration();
    DateTime GetAccessTokenExpiration();
}