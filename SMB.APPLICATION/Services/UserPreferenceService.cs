using SMB.APPLICATION.DTOs.Preferences;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class UserPreferenceService(
    IUserPreferenceRepository preferenceRepository,
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork) : IUserPreferenceService
{
    public async Task<UserPreferenceResponse> GetByUserId(long userId)
    {
        var preference = await preferenceRepository.GetByUserId(userId)
            ?? throw new ResourceNotFoundException("No se encontraron preferencias para el usuario");

        return Map(preference);
    }

    public async Task<UserPreferenceResponse> Update(long userId, UpdateUserPreferenceRequest request)
    {
        if (request.DefaultCurrencyId is null && request.LanguageId is null && request.NotificationsEnabled is null && request.BalanceAlertThreshold is null)
        {
            throw new ValidationException("Debe especificar al menos una preferencia para actualizar");
        }

        var preference = await preferenceRepository.GetByUserId(userId)
            ?? throw new ResourceNotFoundException("No se encontraron preferencias para el usuario");

        if (request.DefaultCurrencyId is not null)
        {
            var currency = await catalogRepository.GetCurrencyById(request.DefaultCurrencyId.Value)
                ?? throw new ResourceNotFoundException("La moneda no existe");
            preference.DefaultCurrencyId = currency.Id;
        }

        if (request.LanguageId is not null)
        {
            var language = await catalogRepository.GetLanguageById(request.LanguageId.Value)
                ?? throw new ResourceNotFoundException("El idioma no existe");
            preference.LanguageId = language.Id;
        }

        if (request.NotificationsEnabled is not null)
        {
            preference.NotificationsEnabled = request.NotificationsEnabled.Value;
        }

        if (request.BalanceAlertThreshold is not null)
        {
            preference.BalanceAlertThreshold = request.BalanceAlertThreshold.Value;
        }

        preference.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();

        var updated = await preferenceRepository.GetByUserId(userId);
        return Map(updated!);
    }

    private static UserPreferenceResponse Map(UserPreference preference) => new()
    {
        UserId = preference.UserId,
        DefaultCurrencyId = preference.DefaultCurrencyId,
        DefaultCurrencyCode = preference.DefaultCurrency.Code,
        LanguageId = preference.LanguageId,
        LanguageCode = preference.Language.Code,
        DarkModeEnabled = preference.DarkModeEnabled,
        NotificationsEnabled = preference.NotificationsEnabled,
        BalanceAlertThreshold = preference.BalanceAlertThreshold,
    };
}
