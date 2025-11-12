using BrewIoT.Shared.Models;
using Refit;

namespace BrewIoT.Client.Recipes;

public interface IRecipeApiService
{
    [Get("/recipe")]
    Task<Recipe[]> GetRecipes();
    
    [Get("/recipe/{id}")]
    Task<Recipe> GetRecipe(int id);
    
    [Post("/recipe")]
    Task<Recipe> CreateRecipe(Recipe recipe);
    
    [Put("/recipe/{id}")]
    Task<Recipe> UpdateRecipe(int id, Recipe recipe);
    
    [Delete("/recipe/{id}")]
    Task DeleteRecipe(int id);
}