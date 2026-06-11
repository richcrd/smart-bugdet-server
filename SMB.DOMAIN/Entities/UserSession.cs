using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class UserSession : BaseEntity
{
    public long UserId { get; set; }
    public string RefreshToken { get; set; } = null!;
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public User User { get; set; } = null!;
}