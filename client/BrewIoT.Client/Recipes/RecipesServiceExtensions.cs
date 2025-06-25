using BrewIoT.Client.Recipes.Pages;
using BrewIoT.Client.Recipes.ViewModels;
using Refit;

namespace BrewIoT.Client.Recipes;

public static class RecipesServiceExtensions
{
    public static IServiceCollection AddRecipes(this IServiceCollection services, string apiUrl)
    {
        services.AddRefitClient<IRecipeApiService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrl));
        
        Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
        
        return services
            .AddTransient<RecipeListPage>()
            .AddTransient<RecipeListPageViewModel>()
            .AddTransient<RecipePage>()
            .AddTransient<RecipePageViewModel>();
    }
}