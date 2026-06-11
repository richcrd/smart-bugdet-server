using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsByUsername(string username);
    Task Add(User user);
    Task<User?> GetByEmailOrPhone(string identifier);
}