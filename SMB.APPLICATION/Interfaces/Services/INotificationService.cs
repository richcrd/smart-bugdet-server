using SMB.APPLICATION.DTOs.Notifications;

namespace SMB.APPLICATION.Interfaces.Services;

public interface INotificationService
{
    Task RegisterDevice(long userId, RegisterDeviceRequest request);
    Task<bool> SendDailyReminderIfNeeded(long userId);
    Task SendDailyReminders();
    Task CheckBalanceAlert(long userId, decimal currentBalance, string currencySymbol);
}
