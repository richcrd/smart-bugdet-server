using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;

namespace SMB.API.BackgroundServices;

public class DailyReminderBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<DailyReminderBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan SendAtLocalTime = new(20, 0, 0);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetDelayUntilNextRun();
            logger.LogInformation("Daily reminder job sleeping for {Delay} until next run", delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            try
            {
                using var scope = scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                await notificationService.SendDailyReminders();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send daily reminders");
            }
        }
    }

    private static TimeSpan GetDelayUntilNextRun()
    {
        var now = AppTimeZone.Now;
        var todayTarget = now.Date.Add(SendAtLocalTime);
        var nextRun = now <= todayTarget ? todayTarget : todayTarget.AddDays(1);
        return nextRun - now;
    }
}
