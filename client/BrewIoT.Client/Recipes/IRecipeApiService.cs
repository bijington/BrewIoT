using Refit;

namespace BrewIoT.Client.Recipes;

public interface IRecipeApiService
{
    [Get("/recipe")]
    Task<Recipe[]> GetRecipes();
    
    [Post("/recipe")]
    Task SaveRecipe(Recipe recipe);
}

public class Recipe
{
    public int Id { get; set; }

    public int Version { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<RecipeStep> Steps { get; set; } = [];
}

public class RecipeStep
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public float? TargetTemperature { get; set; }
}