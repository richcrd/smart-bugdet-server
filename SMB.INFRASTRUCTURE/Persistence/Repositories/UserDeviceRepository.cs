using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class UserDeviceRepository(AppDbContext dbContext) : IUserDeviceRepository
{
    public async Task<UserDevice?> GetByToken(string expoPushToken)
    {
        return await dbContext.UserDevices.FirstOrDefaultAsync(x => x.ExpoPushToken == expoPushToken);
    }

    public async Task<List<string>> GetTokensByUserId(long userId)
    {
        return await dbContext.UserDevices
            .Where(x => x.UserId == userId)
            .Select(x => x.ExpoPushToken)
            .ToListAsync();
    }

    public async Task<List<long>> GetUserIdsWithDevices()
    {
        return await dbContext.UserDevices
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync();
    }

    public async Task AddAsync(UserDevice device)
    {
        await dbContext.UserDevices.AddAsync(device);
    }

    public void Update(UserDevice device)
    {
        dbContext.UserDevices.Update(device);
    }

    public async Task RemoveTokens(List<string> expoPushTokens)
    {
        if (expoPushTokens.Count == 0)
        {
            return;
        }

        var devices = await dbContext.UserDevices
            .Where(x => expoPushTokens.Contains(x.ExpoPushToken))
            .ToListAsync();

        dbContext.UserDevices.RemoveRange(devices);
    }
}
