using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IUserDeviceRepository
{
    Task<UserDevice?> GetByToken(string expoPushToken);
    Task<List<string>> GetTokensByUserId(long userId);
    Task<List<long>> GetUserIdsWithDevices();
    Task AddAsync(UserDevice device);
    void Update(UserDevice device);
    Task RemoveTokens(List<string> expoPushTokens);
}
