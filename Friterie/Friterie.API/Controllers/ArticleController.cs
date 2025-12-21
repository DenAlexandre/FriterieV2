
using Friterie.API.Models;
using Friterie.API.Stores;
using Friterie.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;



[ApiController]
//[Authorize] 
public class ArticleController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IFriterieStore _FriterieStore;
    //https://localhost:5001/FriterieService/BDD/GetAliments

    private const string GET_PRODUCTS_BDD = "/FriterieService/BDD/GetProducts";


    public ArticleController(IConfiguration configuration, IFriterieStore FriterieStore)
    {
        _configuration = configuration;
        _FriterieStore = FriterieStore;
    }



    [HttpGet]
    [Route(GET_PRODUCTS_BDD)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsBDD(int in_type, int in_limit, int in_offset)
    {
        var aliases = await Task.FromResult(await GetProductsBDD(_FriterieStore, in_type, in_limit, in_offset));
        return Ok(aliases);
    }

    private static async Task<List<Product>> GetProductsBDD(IFriterieStore store, int in_type, int in_limit, int in_offset)
    {
        return store is null ? throw new ArgumentNullException(nameof(store)) : await store.GetProducts(in_type, in_limit, in_offset);
    }


}
