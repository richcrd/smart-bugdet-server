using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class CatalogService(ICatalogRepository catalogRepository, IUnitOfWork unitOfWork) : ICatalogService
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

    public async Task<List<CategoriesResponse>> GetUserCategories(long userId)
    {
        return await catalogRepository.GetCategoriesByUserId(userId);
    }

    public async Task<CategoriesResponse> CreateCategory(long userId, CreateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("El nombre es requerido");

        if (await catalogRepository.GetTransactionTypeById(request.TransactionTypeId) is null) 
                throw new ResourceNotFoundException("El tipo de transacción no existe");

        var status = await catalogRepository.GetStatusByCode(StatusCodes.Active)
                     ?? throw new ResourceNotFoundException("El estado activo no existe");

        var category = new Category
        {
            UserId = userId,
            TransactionTypeId = request.TransactionTypeId,
            Name = request.Name.Trim(),
            Icon = request.Icon?.Trim(),
            Color = request.Color?.Trim(),
            IsSystem = false,
            StatusId = status.Id,
            CreatedAt = DateTime.UtcNow
        };

        catalogRepository.AddCategory(category);
        await unitOfWork.SaveChangesAsync();

        return new CategoriesResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Color = category.Color,
            UserId = category.UserId,
            IsSystem = category.IsSystem,
            TransactionTypeId = category.TransactionTypeId,
            Subcategories = []
        };
    }

    public async Task<CategoriesResponse> UpdateCategory(long id, long userId, UpdateCategoryRequest request)
    {
        var category = await catalogRepository.GetOwnedCategory(id, userId)
                       ?? throw new ResourceNotFoundException("Categoría no encontrada o no puedes editarla");

        if (request.Name is not null)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("El nombre no puede estar vacío");
            category.Name = request.Name.Trim();
        }

        if (request.Icon is not null)
            category.Icon = request.Icon.Trim();

        if (request.Color is not null)
            category.Color = request.Color.Trim();

        category.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();

        return new CategoriesResponse
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Color = category.Color,
            UserId = category.UserId,
            IsSystem = category.IsSystem,
            TransactionTypeId = category.TransactionTypeId,
            Subcategories = []
        };
    }

    public async Task DeleteCategory(long id, long userId)
    {
        var category = await catalogRepository.GetOwnedCategory(id, userId)
                       ?? throw new ResourceNotFoundException("Categoria no encontrada o no puedes eliminarla");

        category.Status = await catalogRepository.GetStatusByCode(StatusCodes.Inactive)
                          ?? throw new ResourceNotFoundException("El estado inactivo no existe");
        category.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<SubcategoriesResponse> CreateSubcategory(long userId, CreateSubcategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("El nombre es requerido");

        if (await catalogRepository.GetCategoryById(request.CategoryId) is null)
            throw new ResourceNotFoundException("La categoría no existe");

        var status = await catalogRepository.GetStatusByCode(StatusCodes.Active)
                     ?? throw new ResourceNotFoundException("El estado activo no existe");

        var subcategory = new Subcategory
        {
            CategoryId = request.CategoryId,
            UserId = userId,
            Name = request.Name.Trim(),
            Icon = request.Icon?.Trim(),
            IsSystem = false,
            StatusId = status.Id,
            CreatedAt = DateTime.UtcNow
        };

        catalogRepository.AddSubcategory(subcategory);
        await unitOfWork.SaveChangesAsync();

        return new SubcategoriesResponse
        {
            Id = subcategory.Id,
            Name = subcategory.Name,
            Icon = subcategory.Icon,
            UserId = subcategory.UserId,
            IsSystem = subcategory.IsSystem
        };
    }

    public async Task<SubcategoriesResponse> UpdateSubcategory(long id, long userId, UpdateSubcategoryRequest request)
    {
        var subcategory = await catalogRepository.GetOwnedSubcategory(id, userId)
                          ?? throw new ResourceNotFoundException("Subcategoría no encontrada o no puedes editarla");

        if (request.Name is not null)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("El nombre no puede estar vacío");
            subcategory.Name = request.Name.Trim();
        }

        if (request.Icon is not null)
            subcategory.Icon = request.Icon.Trim();

        subcategory.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();

        return new SubcategoriesResponse
        {
            Id = subcategory.Id,
            Name = subcategory.Name,
            Icon = subcategory.Icon,
            UserId = subcategory.UserId,
            IsSystem = subcategory.IsSystem
        };
    }

    public async Task DeleteSubcategory(long id, long userId)
    {
        var subcategory = await catalogRepository.GetOwnedSubcategory(id, userId)
                          ?? throw new ResourceNotFoundException("Subcategoría no encontrada o no puedes eliminarla");

        subcategory.Status = await catalogRepository.GetStatusByCode(StatusCodes.Inactive)
                             ?? throw new ResourceNotFoundException("El estado inactivo no existe");
        subcategory.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<List<UserPaymentMethodResponse>> GetUserPaymentMethods(long userId)
    {
        return await catalogRepository.GetUserPaymentMethods(userId);
    }

    public async Task<UserPaymentMethodResponse> LinkPaymentMethod(long userId, LinkPaymentMethodRequest request)
    {
        var paymentMethod = await catalogRepository.GetPaymentMethodById(request.PaymentMethodId)
                            ?? throw new ResourceNotFoundException("El método de pago no existe");

        if (await catalogRepository.UserPaymentMethodExists(userId, request.PaymentMethodId))
        {
            throw new DuplicateResourceException("Ya tienes este método de pago vinculado");
        }

        var userPaymentMethod = new UserPaymentMethod()
        {
            UserId = userId,
            PaymentMethodId = request.PaymentMethodId,
            Alias = request.Alias?.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        
        catalogRepository.AddUserPaymentMethod(userPaymentMethod);
        await unitOfWork.SaveChangesAsync();

        return new UserPaymentMethodResponse()
        {
            Id = userPaymentMethod.Id,
            PaymentMethodId = userPaymentMethod.PaymentMethodId,
            Name = paymentMethod.Name,
            Alias = userPaymentMethod.Alias
        };
    }

    public async Task<UserPaymentMethodResponse> UpdatePaymentMethodAlias(long id, long userId, UpdatePaymentMethodAliasRequest request)
    {
        var upm = await catalogRepository.GetUserPaymentMethodById(id)
                  ?? throw new ResourceNotFoundException("Vínculo no encontrado");
        if (upm.UserId != userId)
            throw new ForbiddenException("No puedes editar este método de pago");
        upm.Alias = request.Alias?.Trim();
        upm.CreatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();

        return new UserPaymentMethodResponse()
        {
            Id = upm.Id,
            PaymentMethodId = upm.PaymentMethodId,
            Name = upm.PaymentMethod.Name,
            Alias = upm.Alias
        };
    }

    public async Task UnlinkPaymentMethod(long id, long userId)
    {
        var upm = await catalogRepository.GetUserPaymentMethodById(id)
                  ?? throw new ResourceNotFoundException("Vínculo no encontrado");
        if (upm.UserId != userId)
            throw new ForbiddenException("No puedes eliminar este método de pago");
        catalogRepository.RemoveUserPaymentMethod(upm);
        await unitOfWork.SaveChangesAsync();
    }
}