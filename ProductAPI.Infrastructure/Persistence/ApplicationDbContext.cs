using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    // El constructor recibe las opciones (como la cadena de conexión) desde la API
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Estas propiedades "DbSet" representan las tablas en tu base de datos
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Review> Reviews { get; set; }

    // Aquí configuramos reglas específicas para la base de datos (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Por ejemplo, le decimos a SQL Server que el Precio es un decimal con 2 decimales
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
    }
}