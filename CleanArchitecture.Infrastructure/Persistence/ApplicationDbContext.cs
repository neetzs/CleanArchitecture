using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aca se puede configurar reglas avanzadas de las tablas
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");

        //IMPORTANTE: esto configura las tablas de Identity (AspNetUsers, AspNetRoles, etc)
        base.OnModelCreating(modelBuilder);
    }
}