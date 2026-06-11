using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Subcategory : BaseEntity
{
    public long CategoryId { get; set; }
    public long UserId { get; set; }

    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
    public bool IsSystem { get; set; }
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public Category Category { get; set; } = null!;
    public User? User { get; set; }
}