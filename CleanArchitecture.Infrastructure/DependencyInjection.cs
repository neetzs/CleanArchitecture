using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Infrastructure.Identity;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // --- INICIO BLOQUE NUEVO ---

        // 1. Configurar Auth y Bearer Token (JWT)
        services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);

        // 2. Configurar el núcleo de Identity
        services.AddAuthorizationBuilder();

        services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints(); // <--- Esto habilita las APIs de /register, /login, etc.

        // --- FIN BLOQUE NUEVO ---

        return services;
    }
}