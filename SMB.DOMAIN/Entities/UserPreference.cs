using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class UserPreference : BaseEntity
{
    public long UserId { get; set; }
    public long DefaultCurrencyId { get; set; }
    public long LanguageId { get; set; }
    
    public bool DarkModeEnabled { get; set; }
    public bool NotificationsEnabled { get; set; } = true;

    public User User { get; set; } = null!;
    public Currency DefaultCurrency { get; set; } = null!;
    public Language Language { get; set; } = null!;
}