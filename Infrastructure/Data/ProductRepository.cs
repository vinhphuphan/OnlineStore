// Implementation of the Product Repository using Entity Framework Core.
// Handles querying, adding, updating, and deleting Product entities from the database.
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type == type);

        sort = sort?.Trim().ToLower();
        query = sort switch
        {
            "priceasc" => query.OrderBy(p => p.Price),
            "pricedesc" => query.OrderByDescending(p => p.Price),
            "namedesc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name)
        };

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
        => await context.Products.FindAsync(id);

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
        => await context.Products.Select(p => p.Brand).Distinct().ToListAsync();

    public async Task<IReadOnlyList<string>> GetTypesAsync()
        => await context.Products.Select(p => p.Type).Distinct().ToListAsync();

    public void AddProduct(Product product)
        => context.Products.Add(product);

    public void UpdateProduct(Product product)
    {
        if (context.Entry(product).State == EntityState.Detached)
        {
            context.Products.Attach(product);
        }
        context.Entry(product).State = EntityState.Modified;
    }

    public void DeleteProduct(Product product)
        => context.Products.Remove(product);

    public async Task<bool> ExistsAsync(int id)
        => await context.Products.AnyAsync(p => p.Id == id);

    public async Task<bool> SaveChangesAsync()
        => await context.SaveChangesAsync() > 0;
}
