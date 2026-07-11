using SMB.APPLICATION.DTOs.Catalog;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface ICatalogRepository
{
    Task<Currency?> GetCurrencyByCode(string code);
    Task<Currency?> GetCurrencyById(long id);
    Task<Language?> GetLanguageByCode(string code);
    Task<Language?> GetLanguageById(long id);
    Task<TransactionType?> GetTransactionTypeByCode(string code);
    Task<TransactionType?> GetTransactionTypeById(long id);
    Task<Category?> GetCategoryById(long id);
    Task<Subcategory?> GetSubCategoryById(long id);
    Task<PaymentMethod?> GetPaymentMethodById(long id);
    Task<Status?> GetStatusByCode(string code);
    Task<List<Language>> GetLanguageByActiveStatus();
    Task<List<Currency>> GetCurrenciesByActiveStatus();
    Task<List<CategoriesResponse>> GetCategoriesByActiveStatus();
    Task<List<PaymentMethodResponse>> GetPaymentMethodsByActiveStatus();
}