
using Friterie.API.Models;
using Friterie.API.Stores;

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

    private const string GET_ARTICLES_BDD = "/FriterieService/BDD/GetArticles";


    public ArticleController(IConfiguration configuration, IFriterieStore FriterieStore)
    {
        _configuration = configuration;
        _FriterieStore = FriterieStore;
    }



    [HttpGet]
    [Route(GET_ARTICLES_BDD)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Article>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticlesBDD(int in_type, int in_limit, int in_offset)
    {
        var aliases = await Task.FromResult(await GetArticlesBDD(_FriterieStore, in_type, in_limit, in_offset));
        return Ok(aliases);
    }

    private static async Task<List<Article>> GetArticlesBDD(IFriterieStore store, int in_type, int in_limit, int in_offset)
    {
        return store is null ? throw new ArgumentNullException(nameof(store)) : await store.GetArticles(in_type, in_limit, in_offset);
    }


}
