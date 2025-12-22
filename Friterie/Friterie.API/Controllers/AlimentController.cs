
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
public class AlimentsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IFriterieStore _FriterieStore;
    //https://localhost:5001/FriterieService/BDD/GetAliments
    private const string GET_COUNT_ALIMENTS_BDD = "/FriterieAPI/BDD/GetCountAliments";
    private const string GET_ALIMENTS_BDD = "/FriterieAPI/BDD/GetAliments";
    private const string GET_GROUPES_ALIMENTS_BDD = "/FriterieAPI/BDD/GetGroupesAliments";

    private const string GET_ARTICLES_BDD = "/FriterieAPI/BDD/GetArticles";

    public AlimentsController(IConfiguration configuration, IFriterieStore FriterieStore)
    {
        _configuration = configuration;
        _FriterieStore = FriterieStore;
    }

    [HttpGet]
    [Route(GET_COUNT_ALIMENTS_BDD)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountAlimentsBDD()
    {
        var aliases = await Task.FromResult(await GetCountAlimentsBDD(_FriterieStore));
        return Ok(aliases);
    }

    private static async Task<long> GetCountAlimentsBDD(IFriterieStore store)
    {
        return store is null ? throw new ArgumentNullException(nameof(store)) : await store.GetCountAliments();
    }


    [HttpGet]
    [Route(GET_ALIMENTS_BDD)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Aliment>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GeAlimentsBDD(int in_type, int in_limit, int in_offset)
    {
        var aliases = await Task.FromResult(await GetAlimentsBDD(_FriterieStore, in_type, in_limit, in_offset));
        return Ok(aliases);
    }

    private static async Task<List<Aliment>> GetAlimentsBDD(IFriterieStore store, int in_type, int in_limit, int in_offset)
    {
        return store is null ? throw new ArgumentNullException(nameof(store)) : await store.GetAliments(in_type, in_limit, in_offset);
    }


    [HttpGet]
    [Route(GET_GROUPES_ALIMENTS_BDD)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<int, Dictionary<int, List<GroupeAliment>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupesAlimentsBDD()
    {
        var aliases = await Task.FromResult(await GetGroupesAlimentsBDD(_FriterieStore));
        return Ok(aliases);
    }

    private static async Task<Dictionary<int, Dictionary<int, List<GroupeAliment>>>> GetGroupesAlimentsBDD(IFriterieStore store)
    {
        return store is null ? throw new ArgumentNullException(nameof(store)) : await store.GetGroupesAliments();
    }


}
