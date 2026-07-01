using Microsoft.Extensions.Logging;
using SMB.APPLICATION.DTOs.Notifications;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class NotificationService(
    IUserDeviceRepository deviceRepository,
    ITransactionRepository transactionRepository,
    IUserPreferenceRepository preferenceRepository,
    IExpoPushService expoPushService,
    IUnitOfWork unitOfWork,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task RegisterDevice(long userId, RegisterDeviceRequest request)
    {
        var existing = await deviceRepository.GetByToken(request.ExpoPushToken);

        if (existing is not null)
        {
            existing.UserId = userId;
            existing.Platform = request.Platform;
            existing.UpdatedAt = DateTime.UtcNow;
            deviceRepository.Update(existing);
        }
        else
        {
            await deviceRepository.AddAsync(new UserDevice
            {
                UserId = userId,
                ExpoPushToken = request.ExpoPushToken,
                Platform = request.Platform,
                CreatedAt = DateTime.UtcNow
            });
        }

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> SendDailyReminderIfNeeded(long userId)
    {
        var preference = await preferenceRepository.GetByUserId(userId);

        if (preference is null || !preference.NotificationsEnabled)
        {
            return false;
        }

        var hasTransactionToday = await transactionRepository.ExistsForUserOnDate(userId, AppTimeZone.TodayUtcMidnight);

        if (hasTransactionToday)
        {
            return false;
        }

        var tokens = await deviceRepository.GetTokensByUserId(userId);

        if (tokens.Count == 0)
        {
            return false;
        }

        var invalidTokens = await expoPushService.SendAsync(
            tokens,
            "Recordatorio diario",
            "No has registrado movimientos hoy. ¡No olvides anotar tus gastos e ingresos!");
        await PruneInvalidTokens(invalidTokens);

        logger.LogInformation("Sent daily reminder to user {UserId} ({DeviceCount} device(s))", userId, tokens.Count);

        return true;
    }

    public async Task SendDailyReminders()
    {
        var userIds = await deviceRepository.GetUserIdsWithDevices();

        logger.LogInformation("Running daily reminder job for {UserCount} user(s) with registered devices", userIds.Count);

        foreach (var userId in userIds)
        {
            await SendDailyReminderIfNeeded(userId);
        }
    }

    private async Task PruneInvalidTokens(List<string> invalidTokens)
    {
        if (invalidTokens.Count == 0)
        {
            return;
        }

        await deviceRepository.RemoveTokens(invalidTokens);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Pruned {Count} invalid device token(s)", invalidTokens.Count);
    }
    
    public async Task CheckBalanceAlert(long userId, decimal currentBalance, string currencySymbol)
    {
        var preference = await preferenceRepository.GetByUserId(userId);

        if (preference is null || !preference.NotificationsEnabled || preference.BalanceAlertThreshold is null)
        {
            return;
        }

        if (currentBalance > preference.BalanceAlertThreshold.Value)
        {
            return;
        }

        var tokens = await deviceRepository.GetTokensByUserId(userId);

        if (tokens.Count == 0)
        {
            return;
        }

        var invalidTokens = await expoPushService.SendAsync(
            tokens,
            "Alerta de saldo bajo",
            $"Tu saldo ({currencySymbol} {currentBalance:N2}) llegó a tu límite configurado.");
        
        await PruneInvalidTokens(invalidTokens);
        logger.LogInformation("Sent balance alert to user {UserId} (balance {Balance})", userId, currentBalance);
    }
}
