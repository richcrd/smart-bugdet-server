using SMB.DOMAIN.Entities.Base;

namespace SMB.DOMAIN.Entities;

public class Transaction : BaseEntity
{
    public long UserId { get; set; }
    public long WalletId { get; set; }
    public long TransactionTypeId { get; set; }
    public long CategoryId { get; set; }
    public long? SubcategoryId { get; set; }
    public long? PaymentMethodId { get; set; }
    public long CurrencyId { get; set; }
    
    public decimal Amount { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal AmountInDefaultCurrency { get; set; }
    
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }

    public User User { get; set; } = null!;
    public Wallet Wallet { get; set; } = null!;
    public TransactionType TransactionType { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public Subcategory? Subcategory { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public Currency Currency { get; set; } = null!;
}