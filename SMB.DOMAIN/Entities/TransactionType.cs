using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class TransactionType : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;
}