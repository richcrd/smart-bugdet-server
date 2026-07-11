using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class CatalogService(ICatalogRepository catalogRepository) : ICatalogService
{
    public async Task<List<LanguageResponse>> GetAllLanguages()
    {
        var language = await catalogRepository.GetLanguageByActiveStatus();

        return language.Select(l => new LanguageResponse()
        {
            Id = l.Id,
            Code = l.Code,
            Name = l.Name
        }).ToList();
    }

    public async Task<List<CurrencyResponse>> GetAllCurrencies()
    {
        var currency = await catalogRepository.GetCurrenciesByActiveStatus();

        return currency.Select(m => new CurrencyResponse()
        {
            Id = m.Id,
            Code = m.Code,
            Name = m.Name,
            Symbol = m.Symbol,
            DecimalPlaces = m.DecimalPlaces
        }).ToList();
    }

    public async Task<List<CategoriesResponse>> GetAllCategories()
    {
        return await catalogRepository.GetCategoriesByActiveStatus();
    }
    
    public async Task<List<PaymentMethodResponse>> GetAllPaymentMethods()
    {
        return await catalogRepository.GetPaymentMethodsByActiveStatus();
    }

}