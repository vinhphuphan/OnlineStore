// This class seeds initial product data into the database if it's empty.
// It reads from ../Infrastructure/Data/Seed/products.json and adds to the Products table.

using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            var data = File.ReadAllText("../Infrastructure/Data/Seed/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(data);

            if (products == null) return;

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

    }
}
