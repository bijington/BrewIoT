using System.Collections.ObjectModel;
using BrewIoT.Client.Recipes.Pages;
using BrewIoT.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    
    [RelayCommand]
    private async Task OnRecipeSelected(Recipe recipe)
    {
        try
        {
            await Shell.Current.GoToAsync(
                nameof(RecipePage),
                new Dictionary<string, object> { { "Recipe", recipe } });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [RelayCommand]
    private async Task OnAdd()
    {
        try
        {
            await Shell.Current.GoToAsync(
                nameof(RecipePage),
                new Dictionary<string, object> { { "Recipe", new Recipe() } });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}