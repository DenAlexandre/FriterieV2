namespace Friterie.API.Services;

using Friterie.API.Stores;
using Friterie.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Friterie.Shared.Models.EnumFriterie;

public class ProductService
{

    private readonly IProductStore _productStore;

    public ProductService(IProductStore productStore)
    {
        _productStore = productStore;
    }




    public async Task<List<Product>> GetAllProducts(int type, int limit , int offset) => await _productStore.GetProducts(type, limit, offset);

    public async Task<List<Product>> GetProductsByCategory(ProductTypeEnum category)
    {
        var products = await _productStore.GetProducts((int)ProductTypeEnum.Burgers, 1000, 0);

        return products
            .Where(p => p.TypeProduct.TypeProductCode == (int)category)
            .ToList();
    }

    public async Task<Product?> GetProductById(int id) => await _productStore.GetProductById(id);
}