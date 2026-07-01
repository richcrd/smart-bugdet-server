using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    
    public UserPreference? Preference { get; set; }
    
    public long PeopleId { get; set; }
    public People People { get; set; } = null!;
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public ICollection<UserSession> Sessions { get; set; } = [];
    public ICollection<Wallet> Wallets { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; } = [];
    public ICollection<UserExternalLogin> ExternalLogins { get; set; } = [];
    public ICollection<UserDevice> Devices { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}