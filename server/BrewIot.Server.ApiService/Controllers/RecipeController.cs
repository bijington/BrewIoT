using BrewIoT.Server.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    private readonly ILogger<RecipeController> logger;
    private readonly BrewContext context;

    public RecipeController(ILogger<RecipeController> logger, BrewContext context)
    {
        this.logger = logger;
        this.context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> Get()
    {
        try
        {
            return Ok(await this.context.Recipes.ToListAsync());
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Recipe recipe)
    {
        try
        {
            var newRecipe = new Data.Models.Recipe
            {
                Name = recipe.Name,
                Version = recipe.Version
            };
            
            context.Recipes.Add(newRecipe);
            await context.SaveChangesAsync();
            
            return Ok(recipe);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
}
