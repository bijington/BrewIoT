using System.Collections.ObjectModel;
using BrewIoT.Client.Devices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BrewIoT.Client.Recipes.ViewModels;

public partial class RecipePageViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRecipeApiService recipeApiService;

    [ObservableProperty] 
    private string name = string.Empty;
    
    [ObservableProperty] 
    private ObservableCollection<RecipeStep> steps = [];
    
    [ObservableProperty] 
    private string noReadingsMessage = "No steps.";

    public RecipePageViewModel(IRecipeApiService recipeApiService)
    {
        this.recipeApiService = recipeApiService;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            var recipe = (Recipe)query["Recipe"];
            
            Name = recipe.Name;
            
            //var deviceReadings = await deviceApiService.GetDeviceReadings(recipe.Id);
            
            Steps = new ObservableCollection<RecipeStep>(recipe.Steps);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoReadingsMessage = e.Message;
            throw;
        }
    }

    [RelayCommand]
    private async Task OnSave()
    {
        await this.recipeApiService.SaveRecipe(
            new Recipe
            {
                Name = Name,
                Steps = Steps.ToList()
            });
    }
}