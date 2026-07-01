using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class UserDevice : BaseEntity
{
    public long UserId { get; set; }
    public string ExpoPushToken { get; set; } = null!;
    public string Platform { get; set; } = null!;

    public User User { get; set; } = null!;
}
