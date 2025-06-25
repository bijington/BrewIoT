using BrewIoT.Shared.Models;
using Refit;

namespace BrewIoT.Client.Recipes;

public interface IRecipeApiService
{
    [Get("/recipe")]
    Task<Recipe[]> GetRecipes();
    
    [Post("/recipe")]
    Task SaveRecipe(Recipe recipe);
}