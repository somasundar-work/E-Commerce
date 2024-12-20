using Domain.Entities;
using Domain.Models;
using Infra.Response;

namespace Application.ProductsRepository;

public interface IProductsRepository : IDisposable
{
    public Task<Result<List<ProductDto>>> GetProductsAsync(
        string? brand,
        string? type,
        string? sort
    );
    public Task<Result<List<string>>> GetBrandsAsync();
    public Task<Result<List<string>>> GetTypeAsync();
    public Task<Result<ProductDto>> GetProductByIdAsync(int id);
    public Task<Result<string>> AddProduct(ProductDto product);
    public Task<Result<string>> UpdateProduct(int id, ProductDto product);
    public Task<Result<string>> DeleteProduct(int id);
    public Task<bool> SaveChangesAsync();
}
