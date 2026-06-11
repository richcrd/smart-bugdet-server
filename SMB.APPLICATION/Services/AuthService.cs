using SMB.APPLICATION.DTOs.Auth;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class AuthService(
    IUserRepository userRepository,
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IWalletRepository walletRepository) : IAuthService
{
    public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
    {
        var email = request.Email.Trim().ToLower();
        var username = request.UserName.Trim();

        if (await userRepository.ExistsByEmail(email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }
        
        if (await userRepository.ExistsByUsername(username))
        {
            throw new InvalidOperationException("El username ya está registrado");
        }
        
        var currency = await catalogRepository.GetCurrencyByCode(request.CurrencyCode)
            ?? throw new InvalidOperationException("La moneda no existe");
        
        var language = await catalogRepository.GetLanguageByCode(request.LanguageCode)
                       ?? throw new InvalidOperationException("El idioma no existe");
        
        var status = await catalogRepository.GetStatusByCode("ACTIVE")
                       ?? throw new InvalidOperationException("El estado 'activo' no existe");

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

        await userRepository.Add(user);
        await walletRepository.AddAsync(wallet);
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