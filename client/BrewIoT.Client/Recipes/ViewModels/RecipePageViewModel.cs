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
    private int id;

    [ObservableProperty] 
    private string name = string.Empty;
    
    [ObservableProperty] 
    private int version;
    
    [ObservableProperty] 
    private ObservableCollection<RecipeStepViewModel> steps = [];
    
    [ObservableProperty] 
    private string noStepsMessage = "No steps.";
    
    [ObservableProperty]
    private bool isNewRecipe = true;

    public RecipePageViewModel(IRecipeApiService recipeApiService)
    {
        this.recipeApiService = recipeApiService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            var recipe = (Recipe)query["Recipe"];
            
            Id = recipe.Id;
            Name = recipe.Name;
            Version = recipe.Version;
            IsNewRecipe = recipe.Id == 0;
            
            Steps = new ObservableCollection<RecipeStepViewModel>(
                recipe.Steps.OrderBy(r => r.Order).Select(r => new RecipeStepViewModel
                {
                    Name = r.Name,
                    TargetTemperature = r.TargetTemperature,
                    Duration = r.Duration.TotalHours,
                    Order = r.Order
                }));
            
            UpdateStepOrders();
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
        var newStep = new RecipeStepViewModel
        {
            Order = Steps.Count
        };
        Steps.Add(newStep);
        UpdateStepOrders();
    }
    
    [RelayCommand]
    private void OnRemoveStep(RecipeStepViewModel step)
    {
        Steps.Remove(step);
        UpdateStepOrders();
    }
    
    [RelayCommand]
    private void OnMoveStepUp(RecipeStepViewModel step)
    {
        var index = Steps.IndexOf(step);
        if (index > 0)
        {
            Steps.Move(index, index - 1);
            UpdateStepOrders();
        }
    }
    
    [RelayCommand]
    private void OnMoveStepDown(RecipeStepViewModel step)
    {
        var index = Steps.IndexOf(step);
        if (index < Steps.Count - 1)
        {
            Steps.Move(index, index + 1);
            UpdateStepOrders();
        }
    }
    
    private void UpdateStepOrders()
    {
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[i].Order = i;
        }
    }

    [RelayCommand]
    private async Task OnSave()
    {
        try
        {
            var recipe = new Recipe
            {
                Id = Id,
                Name = Name,
                Version = Version,
                Steps = Steps.Select(s => new RecipeStep
                {
                    Name = s.Name,
                    TargetTemperature = s.TargetTemperature,
                    Duration = TimeSpan.FromHours(s.Duration),
                    Order = s.Order
                }).ToList()
            };
            
            if (IsNewRecipe)
            {
                await this.recipeApiService.CreateRecipe(recipe);
            }
            else
            {
                await this.recipeApiService.UpdateRecipe(Id, recipe);
            }
            
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await Shell.Current.DisplayAlert("Error", $"Failed to save recipe: {e.Message}", "OK");
        }
    }
    
    [RelayCommand]
    private async Task OnDelete()
    {
        try
        {
            if (!IsNewRecipe)
            {
                bool confirmed = await Shell.Current.DisplayAlert(
                    "Delete Recipe",
                    $"Are you sure you want to delete '{Name}'?",
                    "Delete",
                    "Cancel");
                
                if (confirmed)
                {
                    await this.recipeApiService.DeleteRecipe(Id);
                    await Shell.Current.GoToAsync("..");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await Shell.Current.DisplayAlert("Error", $"Failed to delete recipe: {e.Message}", "OK");
        }
    }
}