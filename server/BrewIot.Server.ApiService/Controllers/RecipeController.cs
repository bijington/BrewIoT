using Dapper;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;
using Npgsql;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly NpgsqlConnection connection;
    private readonly ILogger<RecipeController> logger;

    public RecipeController(NpgsqlConnection connection, ILogger<RecipeController> logger)
    {
        this.connection = connection;
        this.logger = logger;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Recipe>> Get()
    {
        try
        {
            const string sql =
                """
                    SELECT Id, Name, Version
                    FROM Recipe
                """;

            return await connection.QueryAsync<Recipe>(sql);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Enumerable.Empty<Recipe>();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Recipe recipe)
    {
        //connection.
        //recipes.Add(recipe);
        
        return Ok(recipe);
    }
}
