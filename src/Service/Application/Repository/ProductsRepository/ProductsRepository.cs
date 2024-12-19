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

    public async Task<Result<string>> CreateProduct(ProductDto request)
    {
        Result<string> result = new();
        var product = mapper.Map<Product>(request);
        await storeContext.AddAsync(product);
        bool isSuccess = await storeContext.SaveChangesAsync() > 0;
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
            isSuccess = await storeContext.SaveChangesAsync() > 0;
        }
        if (isSuccess)
        {
            return result.Success("Updated", "Success", "Product Deleted Successfully");
        }
        return result.Failure("Failure", "unable to delete Product/product not available");
    }

    public async Task<Result<List<ProductDto>>> GetProducts()
    {
        Result<List<ProductDto>> result = new();
        var list = await storeContext
            .Products.Where(p => !p.IsDeleted)
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        if (list == null)
        {
            return result.Failure("Failure", "No Records Found.");
        }
        return result.Success(list, "Success", list.Count.ToString());
    }

    public async Task<Result<ProductDto>> ProductDetail(int id)
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
            bool isSuccess = await storeContext.SaveChangesAsync() > 0;
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
}
