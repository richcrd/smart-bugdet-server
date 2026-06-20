using System.Net.Mail;
using SMB.APPLICATION.DTOs.Auth;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Constants;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class AuthService(
    IUserRepository userRepository,
    ICatalogRepository catalogRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IUserSessionRepository sessionRepository,
    IExternalAuthService externalAuthService,
    IUserExternalLoginRepository externalLoginRepository) : IAuthService
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
        
        var status = await catalogRepository.GetStatusByCode(StatusCodes.Active)
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

    public async Task<LoginResponse> ExternalLogin(ExternalLoginRequest request)
    {
        var provider = request.Provider.Trim().ToUpperInvariant();
        var userInfo = await externalAuthService.VerifyGoogleToken(request.IdToken);

        var existingLogin = await externalLoginRepository.GetByProviderAndUserId(provider, userInfo.ProviderUserId);

        User user;

        if (existingLogin is not null)
        {
            user = existingLogin.User;
        }
        else
        {
            var email = userInfo.Email.Trim().ToLowerInvariant();
            var existingUser = await userRepository.GetByEmailOrPhone(email);

            if (existingUser is not null)
            {
                user = existingUser;
            }
            else
            {
                var status = await catalogRepository.GetStatusByCode(StatusCodes.Active)
                             ?? throw new ResourceNotFoundException("El estado 'activo' no existe");

                var currencyCode = request.CurrencyCode.Trim().ToUpperInvariant();
                var languageCode = request.LanguageCode.Trim().ToUpperInvariant();

                var currency = await catalogRepository.GetCurrencyByCode(currencyCode)
                               ?? throw new ResourceNotFoundException("La moneda no existe");

                var language = await catalogRepository.GetLanguageByCode(languageCode)
                               ?? throw new ResourceNotFoundException("El idioma no existe");

                var nameParts = (userInfo.DisplayName ?? email).Split(' ', 2);
                var now = DateTime.UtcNow;

                var people = new People
                {
                    FirstName = nameParts[0],
                    LastName = nameParts.Length > 1 ? nameParts[1] : "-",
                    StatusId = status.Id,
                    CreatedAt = now
                };

                var username = await GenerateUniqueUsername(email);

                user = new User
                {
                    People = people,
                    Status = status,
                    StatusId = status.Id,
                    Username = username,
                    Email = email,
                    Password = string.Empty,
                    CreatedAt = now
                };

                user.Preference = new UserPreference
                {
                    User = user,
                    DefaultCurrencyId = currency.Id,
                    LanguageId = language.Id,
                    DarkModeEnabled = false,
                    NotificationsEnabled = false,
                    CreatedAt = now
                };

                var wallet = new Wallet
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
            }

            var externalLogin = new UserExternalLogin
            {
                User = user,
                Provider = provider,
                ProviderUserId = userInfo.ProviderUserId,
                Email = userInfo.Email,
                CreatedAt = DateTime.UtcNow
            };

            await externalLoginRepository.Add(externalLogin);
        }

        if (user.Status.Code != StatusCodes.Active)
            throw new ForbiddenException("El usuario está bloqueado o inactivo.");

        var accessToken = tokenService.CreateAccessToken(user);
        var refreshToken = tokenService.CreateRefreshToken();

        var session = new UserSession
        {
            User = user,
            RefreshToken = tokenService.HashRefreshToken(refreshToken),
            DeviceId = request.DeviceId?.Trim(),
            DeviceName = request.DeviceName?.Trim(),
            ExpiresAt = tokenService.GetRefreshTokenExpiration(),
            CreatedAt = DateTime.UtcNow
        };

        user.Sessions.Add(session);
        await unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = tokenService.GetAccessTokenExpiration()
        };
    }

    private async Task<string> GenerateUniqueUsername(string email)
    {
        var baseUsername = email.Split('@')[0].ToLowerInvariant();
        if (baseUsername.Length > 45) baseUsername = baseUsername[..45];

        var username = baseUsername;
        var counter = 1;

        while (await userRepository.ExistsByUsername(username))
            username = $"{baseUsername}{counter++}";

        return username;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var identifier = request.EmailOrPhone.Trim().ToLowerInvariant();

        var user = await userRepository.GetByEmailOrPhone(identifier);

        if (user is null || !passwordHasher.Verify(request.Password, user.Password))
        {
            throw new InvalidCredentialsException();
        }

        if (user.Status.Code != StatusCodes.Active)
        {
            throw new ForbiddenException("El usuario está bloqueado o inactivo.");
        }

        var accessToken = tokenService.CreateAccessToken(user);
        var refreshToken = tokenService.CreateRefreshToken();
        var accessTokenExpiresAt = tokenService.GetAccessTokenExpiration();

        var session = new UserSession
        {
            User = user,
            RefreshToken = tokenService.HashRefreshToken(refreshToken),
            DeviceId = request.DeviceId?.Trim(),
            DeviceName = request.DeviceName?.Trim(),
            ExpiresAt = tokenService.GetRefreshTokenExpiration(),
            CreatedAt = DateTime.UtcNow
        };
        
        user.Sessions.Add(session);
        await unitOfWork.SaveChangesAsync();

        return new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = accessTokenExpiresAt
        };
    }

    public async Task<LoginResponse> Refresh(RefreshTokenRequest request)
    {
        var hash = tokenService.HashRefreshToken(request.RefreshToken);

        var oldSession = await sessionRepository.GetByRefreshToken(hash);

        if (oldSession is null || oldSession.ExpiresAt <= DateTime.UtcNow)
        {
            throw new InvalidCredentialsException();
        }

        if (oldSession.RevokedAt is not null)
        {
            await sessionRepository.RevokeAllByUserId(oldSession.UserId);
            await unitOfWork.SaveChangesAsync();
            throw new InvalidCredentialsException();
        }

        if (oldSession.User.Status.Code != StatusCodes.Active)
        {
            throw new ForbiddenException("El usuario está bloqueado o inactivo");
        }

        var newRefreshToken = tokenService.CreateRefreshToken();
        oldSession.RevokedAt = DateTime.UtcNow;

        var newSession = new UserSession()
        {
            UserId = oldSession.UserId,
            RefreshToken = tokenService.HashRefreshToken(newRefreshToken),
            DeviceId = oldSession.DeviceId,
            DeviceName = oldSession.DeviceName,
            ExpiresAt = tokenService.GetRefreshTokenExpiration(),
            CreatedAt = DateTime.UtcNow
        };

        await sessionRepository.Add(newSession);
        await unitOfWork.SaveChangesAsync();

        return new LoginResponse()
        {
            AccessToken = tokenService.CreateAccessToken(oldSession.User),
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = tokenService.GetAccessTokenExpiration()
        };
    }

    public async Task Logout(string refreshToken, long userId)
    {
        var hash = tokenService.HashRefreshToken(refreshToken);
        var session = await sessionRepository.GetByRefreshToken(hash);

        if (session is null || session.UserId != userId)
        {
            return;
        }

        if (session.RevokedAt is null)
        {
            session.RevokedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task LogoutAll(long userId)
    {
        await sessionRepository.RevokeAllByUserId(userId);
        await unitOfWork.SaveChangesAsync();
    }
}