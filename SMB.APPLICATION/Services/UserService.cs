using System.Net.Mail;
using SMB.APPLICATION.DTOs.User;
using SMB.APPLICATION.Exceptions;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.APPLICATION.Services;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IUserService
{
    public async Task UpdateProfile(long userId, UpdateProfileRequest request)
    {
        var user = await userRepository.GetByIdWithPeople(userId)
                   ?? throw new ResourceNotFoundException("Usuario no encontrado");

        if (request.FirstName is not null)
        {
            if (request.FirstName.Trim().Length is < 1 or > 30)
                throw new ValidationException("El nombre debe tener entre 1 y 30 caracteres");
            user.People.FirstName = request.FirstName.Trim();
        }

        if (request.LastName is not null)
        {
            if (request.LastName.Trim().Length is < 1 or > 30)
                throw new ValidationException("El apellido debe tener entre 1 y 30 caracteres");
            user.People.LastName = request.LastName.Trim();
        }

        if (request.BirthDate.HasValue)
        {
            user.People.BirthDate = request.BirthDate;
        }

        if (request.UserName is not null)
        {
            var username = request.UserName.Trim();
            if (username.Length is < 3 or > 50)
                throw new ValidationException("El usuario debe tener entre 3 y 50 caracteres");

            if (await userRepository.ExistsByUsernameForDifferentUser(username, userId))
                throw new DuplicateResourceException("El nombre de usuario ya está en uso");

            user.Username = username;
        }

        if (request.PhoneNumber is not null)
        {
            var phone = request.PhoneNumber.Trim();
            if (phone.Length > 30)
                throw new ValidationException("El número de teléfono no puede tener más de 30 caracteres");
            user.PhoneNumber = phone;
        }

        if (request.Email is not null)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            if (!MailAddress.TryCreate(email, out _))
                throw new ValidationException("El email no es válido");

            if (await userRepository.ExistsByEmailForDifferentUser(email, userId))
                throw new DuplicateResourceException("El email ya está en uso");

            user.Email = email;
        }

        user.UpdatedAt = DateTime.UtcNow;
        user.People.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();
    }

    public async Task ChangePassword(long userId, ChangePasswordRequest request)
    {
        var user = await userRepository.GetByIdWithPeople(userId)
                   ?? throw new ResourceNotFoundException("Usuario no encontrado");

        if (!passwordHasher.Verify(request.CurrentPassword, user.Password))
            throw new ValidationException("La contraseña actual no es correcta");

        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 12)
            throw new ValidationException("La nueva contraseña debe tener al menos 12 caracteres");

        user.Password = passwordHasher.Hash(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<UserProfileResponse> GetProfile(long userId)
    {
        var user = await userRepository.GetByIdWithPeople(userId)
                   ?? throw new ResourceNotFoundException("Usuario no encontrado");

        return new UserProfileResponse()
        {
            PeopleId = user.PeopleId,
            FirstName = user.People.FirstName,
            LastName = user.People.LastName,
            BirthDate = user.People.BirthDate,
            UserName = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
        };
    }
}
