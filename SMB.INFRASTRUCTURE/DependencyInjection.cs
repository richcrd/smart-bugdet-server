using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.INFRASTRUCTURE.Persistence;
using SMB.INFRASTRUCTURE.Persistence.Repositories;
using SMB.INFRASTRUCTURE.Security;

namespace SMB.INFRASTRUCTURE;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("SMBConnection"));
        });

        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}