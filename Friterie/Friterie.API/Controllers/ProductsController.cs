namespace Friterie.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Friterie.API.Services;
using System.Threading.Tasks;

[ApiController]
[Route("FriterieAPI/api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private const string GET_PRODUCTS_BDD = "/FriterieAPI/api/products/GetProducts";
    private const string GET_PRODUCTS_BY_CATEGORY_BDD = "/FriterieAPI/api/products/category";
    private const string GET_PRODUCTS_BY_ID_BDD = "/FriterieAPI/api/products/";

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet(GET_PRODUCTS_BDD)]
    public async Task<IActionResult> GetAllProducts(int type,  int limit, int offset)
    {
        var products = await _productService.GetAllProducts(type, limit, offset );
        return Ok(products);
    }

    [HttpGet(GET_PRODUCTS_BY_CATEGORY_BDD + "/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category)
    {
        var products = await _productService.GetProductsByCategory(category);
        return Ok(products);
    }

    [HttpGet(GET_PRODUCTS_BY_ID_BDD + "{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }
}