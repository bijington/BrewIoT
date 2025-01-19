using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Server.Data.Models;

namespace BrewIoT.Server.ApiService;

[Route("[controller]")]
[ApiController]
public class RecipeController : ControllerBase
{
    // List<Recipe> _recipes = [];

    // [HttpGet]
    // public async Task<IActionResult> Get()
    // {
    //     return Ok(_recipes);
    // }

    // [HttpPost]
    // public async Task<IResult> Post(Recipe recipe)
    // {
    //     _recipes.Add(recipe);
    //     recipe.Id = _recipes.Count;

    //     Console.WriteLine("Recipe received: ");
        
    //     return Results.Created($"/recipe/{recipe.Id}", recipe);
    // }
}
