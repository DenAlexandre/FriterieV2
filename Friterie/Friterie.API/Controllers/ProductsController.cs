namespace Friterie.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Friterie.API.Services;

[ApiController]
[Route("FriterieAPI/api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("category/{category}")]
    public IActionResult GetProductsByCategory(string category)
    {
        var products = _productService.GetProductsByCategory(category);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }
}