// CleanArchitecture.Application/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Aquí registraremos luego MediatR, Validaciones, etc.
        return services;
    }
}