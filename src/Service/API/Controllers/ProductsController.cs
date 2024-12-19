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
    public async Task<IActionResult> ListAsync()
    {
        return Ok(await productsRepository.GetProducts());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        return Ok(await productsRepository.ProductDetail(id));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ProductDto input)
    {
        return Ok(await productsRepository.CreateProduct(input));
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
