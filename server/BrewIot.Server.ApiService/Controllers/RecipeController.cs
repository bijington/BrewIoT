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
