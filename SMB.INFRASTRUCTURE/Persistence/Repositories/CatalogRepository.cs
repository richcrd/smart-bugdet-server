using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class CatalogRepository(AppDbContext dbContext) : ICatalogRepository
{
    public async Task<Currency?> GetCurrencyByCode(string code)
    {
        return await dbContext.Currencies.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<Currency?> GetCurrencyById(long id)
    {
        return await dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Language?> GetLanguageByCode(string code)
    {
        return await dbContext.Languages.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<Language?> GetLanguageById(long id)
    {
        return await dbContext.Languages.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TransactionType?> GetTransactionTypeByCode(string code)
    {
        return await dbContext.TransactionTypes.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<TransactionType?> GetTransactionTypeById(long id)
    {
        return await dbContext.TransactionTypes.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Category?> GetCategoryById(long id)
    {
        return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Subcategory?> GetSubCategoryById(long id)
    {
        return await dbContext.Subcategories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaymentMethod?> GetPaymentMethodById(long id)
    {
        return await dbContext.PaymentMethods.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Status?> GetStatusByCode(string code)
    {
        return await dbContext.Status.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<List<Language>> GetLanguageByActiveStatus()
    {
        return await dbContext.Languages
            .Where(l => l.Status.Code == StatusCodes.Active)
            .ToListAsync();
    }

    public async Task<List<Currency>> GetCurrenciesByActiveStatus()
    {
        return await dbContext.Currencies
            .Where(m => m.Status.Code == StatusCodes.Active)
            .ToListAsync();
    }

    public async Task<List<CategoriesResponse>> GetCategoriesByActiveStatus()
    {
        return await dbContext.Categories
            .Where(c => c.Status.Code == StatusCodes.Active)
            .Select(c => new CategoriesResponse()
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon,
                Color = c.Color,
                Subcategories = c.Subcategories
                    .Select(s => new SubcategoriesResponse()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Icon = s.Icon
                    }).ToList()
            }).ToListAsync();
    }

    public async Task<List<PaymentMethodResponse>> GetPaymentMethodsByActiveStatus()
    {
        return await dbContext.PaymentMethods
            .Where(p => p.Status.Code == StatusCodes.Active)
            .Select(p => new PaymentMethodResponse()
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync();
    }
}