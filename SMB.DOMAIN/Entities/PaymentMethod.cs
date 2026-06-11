using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class PaymentMethod : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;
}