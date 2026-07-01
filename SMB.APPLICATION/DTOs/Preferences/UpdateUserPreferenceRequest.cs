namespace SMB.APPLICATION.DTOs.Preferences;

public class UpdateUserPreferenceRequest
{
    public long? DefaultCurrencyId { get; set; }
    public long? LanguageId { get; set; }
    public bool? NotificationsEnabled { get; set; }
}
