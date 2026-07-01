using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class NotificationRepository(AppDbContext dbContext) : INotificationRepository
{
    public async Task<List<Notification>> GetByUserId(long userId)
    {
        return await dbContext.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(50)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdForUser(long id, long userId)
    {
        return await dbContext.Notifications
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task AddAsync(Notification notification)
    {
        await dbContext.Notifications.AddAsync(notification);
    }

    public async Task MarkAllAsRead(long userId)
    {
        var unread = await dbContext.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ToListAsync();

        foreach (var notification in unread)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }
    }
}
