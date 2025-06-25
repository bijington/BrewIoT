using Dapper;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;
using Npgsql;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    // private readonly NpgsqlConnection connection;
    private readonly ILogger<RecipeController> logger;

    public RecipeController(ILogger<RecipeController> logger)
    {
        // this.connection = connection;
        this.logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> Get()
    {
        try
        {
            var recipe = new Recipe
            {
                Name = "IPA",
                Version = 14,
                Steps = 
                [
                    new RecipeStep
                    {
                        Name = "Fermentation",
                        TargetTemperature = 18,
                        Duration = new TimeSpan(14, 0, 2, 0)
                    },
                    new RecipeStep
                    {
                        Name = "Carbonation",
                        TargetTemperature = 20,
                        Duration = new TimeSpan(14, 2, 0, 0)
                    },
                    new RecipeStep
                    {
                        Name = "Cold crash",
                        TargetTemperature = 2,
                        Duration = new TimeSpan(7, 0, 0, 0)
                    }
                ]
            };
            
            const string sql =
                """
                    SELECT Id, Name, Version
                    FROM Recipe
                """;

            return Ok(new List<Recipe> { recipe }); //await connection.QueryAsync<Recipe>(sql);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest(Enumerable.Empty<Recipe>());
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
