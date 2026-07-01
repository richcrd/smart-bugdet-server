namespace SMB.APPLICATION.DTOs.Preferences;

public class UserPreferenceResponse
{
    public long UserId { get; set; }
    public long DefaultCurrencyId { get; set; }
    public string DefaultCurrencyCode { get; set; } = null!;
    public long LanguageId { get; set; }
    public string LanguageCode { get; set; } = null!;
    public bool DarkModeEnabled { get; set; }
    public bool NotificationsEnabled { get; set; }
    public decimal? BalanceAlertThreshold { get; set; }
}
