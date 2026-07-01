namespace SMB.DOMAIN.Constants;

public static class AppTimeZone
{
    public static readonly TimeSpan Offset = TimeSpan.FromHours(-6);

    public static DateTime Now => DateTime.UtcNow.Add(Offset);

    public static DateTime TodayUtcMidnight
    {
        get
        {
            var now = Now;
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        }
    }
}
