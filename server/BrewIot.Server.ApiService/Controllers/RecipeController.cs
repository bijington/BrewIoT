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
            var recipes = await this.context.Recipes
                .Include(r => r.Steps)
                .ToListAsync();
            
            return Ok(recipes.Select(r => new Recipe
            {
                Id = r.Id,
                Name = r.Name,
                Version = r.Version,
                Steps = r.Steps.OrderBy(s => s.Order).Select(s => new RecipeStep
                {
                    Id = s.Id,
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = s.Duration,
                    Order = s.Order
                }).ToList()
            }));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving recipes");
            return StatusCode(500, "An error occurred while retrieving recipes");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetById(int id)
    {
        try
        {
            var recipe = await this.context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (recipe == null)
            {
                return NotFound();
            }
            
            return Ok(new Recipe
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Version = recipe.Version,
                Steps = recipe.Steps.OrderBy(s => s.Order).Select(s => new RecipeStep
                {
                    Id = s.Id,
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = s.Duration,
                    Order = s.Order
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving recipe {RecipeId}", id);
            return StatusCode(500, "An error occurred while retrieving the recipe");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Recipe>> Post([FromBody] Recipe recipe)
    {
        try
        {
            var dbRecipe = new Data.Models.Recipe
            {
                Name = recipe.Name,
                Version = recipe.Version,
                Steps = recipe.Steps.Select((s, index) => new Data.Models.RecipeStep
                {
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = s.Duration,
                    Order = index
                }).ToList()
            };
            
            this.context.Recipes.Add(dbRecipe);
            await this.context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = dbRecipe.Id }, new Recipe
            {
                Id = dbRecipe.Id,
                Name = dbRecipe.Name,
                Version = dbRecipe.Version,
                Steps = dbRecipe.Steps.OrderBy(s => s.Order).Select(s => new RecipeStep
                {
                    Id = s.Id,
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = s.Duration,
                    Order = s.Order
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating recipe");
            return StatusCode(500, "An error occurred while creating the recipe");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Recipe>> Put(int id, [FromBody] Recipe recipe)
    {
        try
        {
            var existingRecipe = await this.context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (existingRecipe == null)
            {
                return NotFound();
            }
            
            existingRecipe.Name = recipe.Name;
            existingRecipe.Version = recipe.Version;
            
            // Remove existing steps
            this.context.RecipeSteps.RemoveRange(existingRecipe.Steps);
            
            // Add updated steps
            existingRecipe.Steps = recipe.Steps.Select((s, index) => new Data.Models.RecipeStep
            {
                Name = s.Name,
                TargetTemperature = s.TargetTemperature,
                Duration = s.Duration,
                Order = index,
                RecipeId = id
            }).ToList();
            
            await this.context.SaveChangesAsync();
            
            return Ok(new Recipe
            {
                Id = existingRecipe.Id,
                Name = existingRecipe.Name,
                Version = existingRecipe.Version,
                Steps = existingRecipe.Steps.OrderBy(s => s.Order).Select(s => new RecipeStep
                {
                    Id = s.Id,
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = s.Duration,
                    Order = s.Order
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipe {RecipeId}", id);
            return StatusCode(500, "An error occurred while updating the recipe");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var recipe = await this.context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (recipe == null)
            {
                return NotFound();
            }
            
            this.context.Recipes.Remove(recipe);
            await this.context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting recipe {RecipeId}", id);
            return StatusCode(500, "An error occurred while deleting the recipe");
        }
    }
}
