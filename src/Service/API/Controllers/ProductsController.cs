using Application.ProductsRepository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsRepository productsRepository;

    public ProductsController(IProductsRepository productsRepository)
    {
        this.productsRepository = productsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync(
        [FromQuery] string? brand,
        [FromQuery] string? type,
        [FromQuery] string? sort
    )
    {
        return Ok(await productsRepository.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("Brands")]
    public async Task<IActionResult> ListBrandsAsync()
    {
        return Ok(await productsRepository.GetBrandsAsync());
    }

    [HttpGet("Types")]
    public async Task<IActionResult> ListTypesAsync()
    {
        return Ok(await productsRepository.GetTypeAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        return Ok(await productsRepository.GetProductByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ProductDto input)
    {
        return Ok(await productsRepository.AddProduct(input));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] ProductDto input)
    {
        return Ok(await productsRepository.UpdateProduct(id, input));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        return Ok(await productsRepository.DeleteProduct(id));
    }
}
