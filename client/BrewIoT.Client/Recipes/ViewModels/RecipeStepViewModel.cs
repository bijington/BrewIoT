using CommunityToolkit.Mvvm.ComponentModel;

namespace BrewIoT.Client.Recipes.ViewModels;

public partial class RecipeStepViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;
    
    [ObservableProperty]
    private double? targetTemperature;
    
    [ObservableProperty]
    private double duration;
    
    [ObservableProperty]
    private int order;
}