using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class UserPaymentMethod : BaseEntity
{
    public long UserId { get; set; }
    public long PaymentMethodId { get; set; }
    
    public string? Alias { get; set; }

    public User User { get; set; } = null!;
    public PaymentMethod PaymentMethod { get; set; } = null!;
}