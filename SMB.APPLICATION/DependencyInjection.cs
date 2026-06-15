using Microsoft.Extensions.DependencyInjection;
using SMB.APPLICATION.Interfaces.Services;
using SMB.APPLICATION.Services;

namespace SMB.APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICatalogService, CatalogService>();
        return services;
    }
}