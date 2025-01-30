using System.Collections.ObjectModel;
using BrewIoT.Client.Recipes.Pages;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BrewIoT.Client.Recipes.ViewModels;

public partial class RecipeListPageViewModel : ObservableObject
{
    private readonly IRecipeApiService recipeApiService;

    [ObservableProperty] 
    private ObservableCollection<Recipe> recipes = [];
    
    [ObservableProperty] 
    private string noRecipesMessage = "No recipes available.";

    public RecipeListPageViewModel(IRecipeApiService recipeApiService)
    {
        this.recipeApiService = recipeApiService;
    }

    public async void OnNavigatedTo()
    {
        try
        {
            var availableRecipes = await recipeApiService.GetRecipes();
            
            Recipes = new ObservableCollection<Recipe>(availableRecipes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoRecipesMessage = e.Message;
            throw;
        }
        
    }

    private async Task OnAdd()
    {
        await Shell.Current.GoToAsync($"{nameof(RecipePage)}");
    }
}