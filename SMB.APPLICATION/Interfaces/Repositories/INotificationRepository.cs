using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetByUserId(long userId);
    Task<Notification?> GetByIdForUser(long id, long userId);
    Task AddAsync(Notification notification);
    Task MarkAllAsRead(long userId);
}
