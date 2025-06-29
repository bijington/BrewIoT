using System.Collections.ObjectModel;
using BrewIoT.Client.Devices;
using BrewIoT.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BrewIoT.Client.Recipes.ViewModels;

public partial class RecipePageViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRecipeApiService recipeApiService;

    [ObservableProperty] 
    private string name = string.Empty;
    
    [ObservableProperty] 
    private int version;
    
    [ObservableProperty] 
    private ObservableCollection<RecipeStepViewModel> steps = [];
    
    [ObservableProperty] 
    private string noStepsMessage = "No steps.";

    public RecipePageViewModel(IRecipeApiService recipeApiService)
    {
        this.recipeApiService = recipeApiService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            var recipe = (Recipe)query["Recipe"];
            
            Name = recipe.Name;
            Version = recipe.Version;
            
            Steps = new ObservableCollection<RecipeStepViewModel>(
                recipe.Steps.Select(r => new RecipeStepViewModel
                {
                    Name = r.Name,
                    TargetTemperature = r.TargetTemperature,
                    Duration = r.Duration.TotalHours
                }));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoStepsMessage = e.Message;
            throw;
        }
    }

    [RelayCommand]
    private void OnAddStep()
    {
        Steps.Add(new RecipeStepViewModel());
    }
    
    [RelayCommand]
    private void OnRemoveStep(RecipeStepViewModel step)
    {
        Steps.Remove(step);
    }

    [RelayCommand]
    private async Task OnSave()
    {
        await this.recipeApiService.SaveRecipe(
            new Recipe
            {
                Name = Name,
                Steps = Steps.Select(s => new RecipeStep
                {
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = TimeSpan.FromHours(s.Duration)
                }).ToList()
            });
    }
}