using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Context;
using Domain.Entities;
using Domain.Models;
using Infra.Response;
using Microsoft.EntityFrameworkCore;

namespace Application.ProductsRepository;

public class ProductsRepository : IProductsRepository
{
    private readonly StoreContext storeContext;
    private readonly IMapper mapper;

    public ProductsRepository(StoreContext storeContext, IMapper mapper)
    {
        this.storeContext = storeContext;
        this.mapper = mapper;
    }

    public async Task<Result<string>> AddProduct(ProductDto request)
    {
        Result<string> result = new();
        var product = mapper.Map<Product>(request);
        await storeContext.AddAsync(product);
        bool isSuccess = await SaveChangesAsync();
        if (isSuccess)
        {
            return result.Success("Created", "Success", "Product Created Successfully");
        }
        return result.Failure("Failure", "unable to create Product");
    }

    public async Task<Result<string>> DeleteProduct(int id)
    {
        Result<string> result = new();
        var product = await storeContext.Products.FindAsync(id);
        bool isSuccess = false;
        if (product != null && !product.IsDeleted)
        {
            product.IsDeleted = true;
            storeContext.Update(product);
            isSuccess = await SaveChangesAsync();
        }
        if (isSuccess)
        {
            return result.Success("Updated", "Success", "Product Deleted Successfully");
        }
        return result.Failure("Failure", "unable to delete Product/product not available");
    }

    public async Task<Result<List<ProductDto>>> GetProductsAsync(
        string? brand,
        string? type,
        string? sort
    )
    {
        Result<List<ProductDto>> result = new();
        var query = storeContext.Products.Where(p => !p.IsDeleted).AsQueryable();
        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(x => x.Brand == brand);
        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(x => x.Type == type);

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Name),
        };

        var products = await query
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        if (products == null)
        {
            return result.Failure("Failure", "No Records Found.");
        }
        return result.Success(products, "Success", products.Count.ToString());
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
    {
        Result<ProductDto> result = new();
        var data = await storeContext.Products.SingleOrDefaultAsync(x =>
            x.Id == id && !x.IsDeleted
        );
        if (data != null)
        {
            var product = mapper.Map<ProductDto>(data);
            return result.Success(product, "Success", "Product Details");
        }
        return result.Failure("Failure", "no products found");
    }

    public async Task<Result<string>> UpdateProduct(int id, ProductDto request)
    {
        Result<string> result = new();
        var product = await storeContext.Products.SingleOrDefaultAsync(p =>
            p.Id == id && !p.IsDeleted
        );
        if (product != null)
        {
            product = mapper.Map(request, product);
            bool isSuccess = await SaveChangesAsync();
            if (isSuccess)
            {
                return result.Success("Created", "Success", "Product Updated Successfully");
            }
        }
        return result.Failure("Failure", "unable to update Product/product not available");
    }

    public void Dispose()
    {
        storeContext.Dispose();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await storeContext.SaveChangesAsync() > 0;
    }

    public async Task<Result<List<string>>> GetBrandsAsync()
    {
        Result<List<string>> result = new();
        List<string> brands = [];
        brands = await storeContext.Products.Select(b => b.Brand ?? "").Distinct().ToListAsync();
        if (brands != null && brands.Count > 0)
            return result.Success(brands, "Success", brands.Count.ToString());
        brands = [];
        return result.Success(brands, "Success", brands.Count.ToString());
    }

    public async Task<Result<List<string>>> GetTypeAsync()
    {
        Result<List<string>> result = new();
        List<string> types = [];
        types = await storeContext.Products.Select(b => b.Type ?? "").Distinct().ToListAsync();
        if (types != null && types.Count > 0)
            return result.Success(types, "Success", types.Count.ToString());
        types = [];
        return result.Success(types, "Success", types.Count.ToString());
    }
}
