using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    // Aca se pueden agregar propiedades adicionales para el usuario
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}