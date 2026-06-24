using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface ICatalogRepository
{
    Task<Currency?> GetCurrencyByCode(string code);
    Task<Language?> GetLanguageByCode(string code);
    Task<TransactionType?> GetTransactionTypeByCode(string code);
    Task<TransactionType?> GetTransactionTypeById(long id);
    Task<Category?> GetCategoryById(long id);
    Task<Subcategory?> GetSubCategoryById(long id);
    Task<PaymentMethod?> GetPaymentMethodById(long id);
    Task<Status?> GetStatusByCode(string code);
    Task<List<Language>> GetLanguageByActiveStatus();
    Task<List<Currency>> GetCurrenciesByActiveStatus();
}