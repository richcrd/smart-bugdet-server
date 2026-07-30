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
                UserId = c.UserId,
                IsSystem = c.IsSystem,
                TransactionTypeId = c.TransactionTypeId,
                Subcategories = c.Subcategories
                    .Select(s => new SubcategoriesResponse()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Icon = s.Icon,
                        UserId = s.UserId,
                        IsSystem = s.IsSystem
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

    public async Task<List<CategoriesResponse>> GetCategoriesByUserId(long userId)
    {
        return await dbContext.Categories
            .Where(c => c.Status.Code == StatusCodes.Active
                        && (c.IsSystem || c.UserId == userId))
            .Select(c => new CategoriesResponse()
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon,
                Color = c.Color,
                UserId = c.UserId,
                IsSystem = c.IsSystem,
                TransactionTypeId = c.TransactionTypeId,
                Subcategories = c.Subcategories
                    .Where(s => s.Status.Code == StatusCodes.Active
                                && (s.IsSystem || s.UserId == userId))
                    .Select(s => new SubcategoriesResponse()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Icon = s.Icon,
                        UserId = s.UserId,
                        IsSystem = s.IsSystem
                    }).ToList()
            }).ToListAsync();
    }

    public async Task<Category?> GetOwnedCategory(long id, long userId)
    {
        return await dbContext.Categories
            .Include(c => c.Status)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId && !c.IsSystem);
    }

    public async Task<Subcategory?> GetOwnedSubcategory(long id, long userId)
    {
        return await dbContext.Subcategories
            .Include(s => s.Status)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId && !s.IsSystem);
    }

    public async Task<List<UserPaymentMethodResponse>> GetUserPaymentMethods(long userId)
    {
        return await dbContext.UserPaymentMethods
            .Where(upm => upm.UserId == userId)
            .Select(upm => new UserPaymentMethodResponse()
            {
                Id = upm.Id,
                PaymentMethodId = upm.PaymentMethodId,
                Name = upm.PaymentMethod.Name,
                Alias = upm.Alias
            }).ToListAsync();
    }

    public async Task<UserPaymentMethod?> GetUserPaymentMethodById(long id)
    {
        return await dbContext.UserPaymentMethods
            .Include(upm => upm.PaymentMethod)
            .FirstOrDefaultAsync(upm => upm.Id == id);
    }

    public async Task<UserPaymentMethod?> GetUserPaymentMethodByUserIdAndMethodId(long userId, long paymentMethodId)
    {
        return await dbContext.UserPaymentMethods
            .FirstOrDefaultAsync(upm => upm.UserId == userId && upm.PaymentMethodId == paymentMethodId);
    }

    public async Task<bool> UserPaymentMethodExists(long userId, long paymentMethodId)
    {
        return await dbContext.UserPaymentMethods
            .AnyAsync(upm => upm.UserId == userId && upm.PaymentMethodId == paymentMethodId);
    }

    public void AddCategory(Category category)
    {
        dbContext.Categories.Add(category);
    }

    public void AddSubcategory(Subcategory subcategory)
    {
        dbContext.Subcategories.Add(subcategory);
    }

    public void AddUserPaymentMethod(UserPaymentMethod userPaymentMethod)
    {
        dbContext.UserPaymentMethods.Add(userPaymentMethod);
    }

    public void RemoveUserPaymentMethod(UserPaymentMethod userPaymentMethod)
    {
        dbContext.UserPaymentMethods.Remove(userPaymentMethod);
    }
}