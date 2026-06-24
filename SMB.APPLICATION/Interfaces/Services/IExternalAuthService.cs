using SMB.APPLICATION.DTOs.Auth;

namespace SMB.APPLICATION.Interfaces.Services;

public interface IExternalAuthService
{
    Task<ExternalUserInfo> VerifyGoogleToken(string idToken);
}