using System.Net.Mail;
using SMB.APPLICATION.DTOs.Auth;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class AuthService(
    IUserRepository userRepository,
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IAuthService
{
    public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.UserName.Trim();
        var currencyCode = request.CurrencyCode.Trim().ToUpperInvariant();
        var languageCode = request.LanguageCode.Trim().ToUpperInvariant();

        if (!MailAddress.TryCreate(email, out _))
        {
            throw new ValidationException("El email no es válido");
        }

        if (username.Length is < 3 or > 50)
        {
            throw new ValidationException("El usuario debe tener entre 3 y 50 caracteres");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 12)
        {
            throw new ValidationException("La contraseña debe tener al menos 12 carácteres");
        }

        if (await userRepository.ExistsByEmail(email))
        {
            throw new DuplicateResourceException("El email ya está registrado");
        }
        
        if (await userRepository.ExistsByUsername(username))
        {
            throw new DuplicateResourceException("El usuario ya está registrado");
        }
        
        var currency = await catalogRepository.GetCurrencyByCode(currencyCode)
            ?? throw new ResourceNotFoundException("La moneda no existe");
        
        var language = await catalogRepository.GetLanguageByCode(languageCode)
                       ?? throw new ResourceNotFoundException("El idioma no existe");
        
        var status = await catalogRepository.GetStatusByCode("ACTIVE")
                       ?? throw new ResourceNotFoundException("El estado 'activo' no existe");

        var now = DateTime.UtcNow;

        var people = new People()
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            BirthDate = request.BirthDate,
            StatusId = status.Id,
            CreatedAt = now
        };

        var user = new User()
        {
            People = people,
            StatusId = status.Id,
            Username = username,
            Email = email,
            Password = passwordHasher.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            CreatedAt = now
        };

        user.Preference = new UserPreference()
        {
            User = user,
            DefaultCurrencyId = currency.Id,
            LanguageId = language.Id,
            DarkModeEnabled = false,
            NotificationsEnabled = false,
            CreatedAt = now
        };

        var wallet = new Wallet()
        {
            User = user,
            StatusId = status.Id,
            CurrencyId = currency.Id,
            Name = "Cartera Principal",
            InitialBalance = 0,
            CurrentBalance = 0,
            IsDefault = true,
            CreatedAt = now
        };

        user.Wallets.Add(wallet);
        await userRepository.Add(user);
        await unitOfWork.SaveChangesAsync();

        return new RegisterUserResponse()
        {
            PeopleId = people.Id,
            UserId = user.Id,
            WalletId = wallet.Id,
            FullName = $"{people.FirstName} {people.LastName}",
            UserName = user.Username,
            Email = user.Email
        };
    }
}