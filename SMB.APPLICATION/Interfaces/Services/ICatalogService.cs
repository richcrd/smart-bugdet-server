using SMB.APPLICATION.DTOs.Catalog;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Services;

public interface ICatalogService
{
    Task<List<LanguageResponse>> GetAllLanguages();
    Task<List<CurrencyResponse>> GetAllCurrencies();
    Task<List<CategoriesResponse>> GetAllCategories();
    Task<List<PaymentMethodResponse>> GetAllPaymentMethods();
}