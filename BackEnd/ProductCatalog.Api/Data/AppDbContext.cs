// Data/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Demo", Description = "Seed item", Price = 9.99m, Category = "Sample" }
        );
    }
}