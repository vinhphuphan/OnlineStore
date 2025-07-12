// Defines the Entity Framework Core DbContext for the application.
// Registers DbSet<Product> and applies entity configurations from the assembly.

using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 👇 Configure for Price of Product
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
    }
}
