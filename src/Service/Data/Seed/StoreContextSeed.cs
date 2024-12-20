using System.Text.Json;
using Data.Context;
using Domain.Entities;

namespace Data.Seed;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext storeContext)
    {
        if (!storeContext.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../../Service/Data/Seed/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products == null)
                return;

            await storeContext.Products.AddRangeAsync(products);
            await storeContext.SaveChangesAsync();
        }
    }
}
