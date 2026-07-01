using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Notification : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    public User User { get; set; } = null!;
}
