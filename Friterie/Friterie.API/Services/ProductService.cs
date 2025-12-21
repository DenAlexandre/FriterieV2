namespace Friterie.API.Services;

using Friterie.API.Models;
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Linq;

public class ProductService
{
    private readonly DataService _dataService;

    public ProductService(DataService dataService)
    {
        _dataService = dataService;
    }

    public List<Product> GetAllProducts() => _dataService.GetAllProducts();

    public List<Product> GetProductsByCategory(string category) =>
        _dataService.GetAllProducts().Where(p => p.TypeProduct.TypeProductNom == category).ToList();

    public Product? GetProductById(int id) => _dataService.GetProductById(id);
}