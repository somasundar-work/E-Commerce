using Domain.Entities;
using Domain.Models;
using Infra.Response;

namespace Application.ProductsRepository;

public interface IProductsRepository : IDisposable
{
    public Task<Result<List<ProductDto>>> GetProducts();
    public Task<Result<ProductDto>> ProductDetail(int id);
    public Task<Result<string>> CreateProduct(ProductDto product);
    public Task<Result<string>> UpdateProduct(int id, ProductDto product);
    public Task<Result<string>> DeleteProduct(int id);
}
