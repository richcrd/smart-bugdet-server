using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class UserExternalLogin : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; } = null!;

    public string Provider { get; set; } = null!;
    public string ProviderUserId { get; set; } = null!;
    public string Email { get; set; } = null!;
}