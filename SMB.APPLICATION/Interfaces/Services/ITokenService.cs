using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Services;

public interface ITokenService
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
    DateTime GetAccessTokenExpiration();
}