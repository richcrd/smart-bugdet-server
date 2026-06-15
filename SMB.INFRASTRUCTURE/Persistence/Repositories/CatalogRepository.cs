using Microsoft.EntityFrameworkCore;
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

    public async Task<Language?> GetLanguageByCode(string code)
    {
        return await dbContext.Languages.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<TransactionType?> GetTransactionTypeByCode(string code)
    {
        return await dbContext.TransactionTypes.FirstOrDefaultAsync(x => x.Code == code);
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
}