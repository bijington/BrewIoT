using BrewIoT.Client.Recipes.Pages;
using BrewIoT.Client.Recipes.ViewModels;
using Refit;

namespace BrewIoT.Client.Recipes;

public static class RecipesServiceExtensions
{
    public static IServiceCollection AddRecipes(this IServiceCollection services)
    {
        services.AddRefitClient<IRecipeApiService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7466"));
        
        return services
            .AddTransient<RecipeListPage>()
            .AddTransient<RecipeListPageViewModel>();
    }
}