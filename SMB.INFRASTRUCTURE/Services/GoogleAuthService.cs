using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using SMB.APPLICATION.DTOs.Auth;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.INFRASTRUCTURE.Services;

public class GoogleAuthService(IConfiguration configuration) : IExternalAuthService
{
    public async Task<ExternalUserInfo> VerifyGoogleToken(string idToken)
    {
        var clientIds = configuration.GetSection("Google:ClientIds").Get<string[]>();

        if (clientIds is null || clientIds.Length == 0)
            throw new InvalidOperationException("Google:ClientIds no está configurado");

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = clientIds
            });
        }
        catch (InvalidJwtException)
        {
            throw new InvalidCredentialsException();
        }

        return new ExternalUserInfo
        {
            ProviderUserId = payload.Subject,
            Email = payload.Email,
            DisplayName = payload.Name
        };
    }
}
