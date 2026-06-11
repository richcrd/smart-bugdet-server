using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Wallet : BaseEntity
{
    public long UserId { get; set; }
    public long CurrencyId { get; set; }

    public string Name { get; set; } = null!;
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IsDefault { get; set; }
    
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public User User { get; set; } = null!;
    public Currency Currency { get; set; } = null!;

    public ICollection<Transaction> Transactions { get; set; } = [];
}