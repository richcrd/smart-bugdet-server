using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Category : BaseEntity
{
    public long UserId { get; set; }
    public long TransactionTypeId { get; set; }

    public string Name { get; set; } = null!;
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool IsSystem { get; set; }
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;
    
    public User? User { get; set; }
    public TransactionType TransactionType { get; set; } = null!;

    public ICollection<Subcategory> Subcategories { get; set; } = [];
}