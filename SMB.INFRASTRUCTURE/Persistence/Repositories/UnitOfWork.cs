using SMB.APPLICATION.Interfaces.Repositories;

namespace SMB.INFRASTRUCTURE.Persistence.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}