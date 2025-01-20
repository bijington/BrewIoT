using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    public static readonly List<Recipe> recipes = [];
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(recipes);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Recipe recipe)
    {
        recipes.Add(recipe);
        
        return Ok(recipe);
    }
}
