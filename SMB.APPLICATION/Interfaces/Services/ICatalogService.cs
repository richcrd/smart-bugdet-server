using SMB.APPLICATION.DTOs.Catalog;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Services;

public interface ICatalogService
{
    Task<List<LanguageResponse>> GetAllLanguages();
    Task<List<CurrencyResponse>> GetAllCurrencies();
    Task<List<CategoriesResponse>> GetAllCategories();
    Task<List<PaymentMethodResponse>> GetAllPaymentMethods();
    
    Task<List<CategoriesResponse>> GetUserCategories(long userId);
    Task<CategoriesResponse> CreateCategory(long userId, CreateCategoryRequest request);
    Task<CategoriesResponse> UpdateCategory(long id, long userId, UpdateCategoryRequest request);
    Task DeleteCategory(long id, long userId);
    Task<SubcategoriesResponse> CreateSubcategory(long userId, CreateSubcategoryRequest request);
    Task<SubcategoriesResponse> UpdateSubcategory(long id, long userId, UpdateSubcategoryRequest request);
    Task DeleteSubcategory(long id, long userId);
    Task<List<UserPaymentMethodResponse>> GetUserPaymentMethods(long userId);
    Task<UserPaymentMethodResponse> LinkPaymentMethod(long userId, LinkPaymentMethodRequest request);
    Task<UserPaymentMethodResponse> UpdatePaymentMethodAlias(long id, long userId, UpdatePaymentMethodAliasRequest request);
    Task UnlinkPaymentMethod(long id, long userId);
}