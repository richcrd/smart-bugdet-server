using Microsoft.EntityFrameworkCore;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.DOMAIN.Entities;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<bool> ExistsByEmail(string email)
    {
        return await dbContext.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByUsername(string username)
    {
        return await dbContext.Users.AnyAsync(x => x.Username == username);
    }

    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }
}