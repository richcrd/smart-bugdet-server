using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsByUsername(string username);
    Task Add(User user);
    Task<User?> GetByEmailOrPhone(string identifier);
    Task<User?> GetByIdWithPeople(long id);
    Task<bool> ExistsByEmailForDifferentUser(string email, long userId);
    Task<bool> ExistsByUsernameForDifferentUser(string username, long userId);
}