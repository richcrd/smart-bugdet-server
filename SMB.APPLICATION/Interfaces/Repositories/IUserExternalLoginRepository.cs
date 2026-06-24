using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IUserExternalLoginRepository
{
    Task<UserExternalLogin?> GetByProviderAndUserId(string provider, string providerUserId);
    Task Add(UserExternalLogin externalLogin);
}
