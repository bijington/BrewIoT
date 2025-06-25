using BrewIoT.Client.Recipes.ViewModels;

namespace BrewIoT.Client.Recipes.Pages;

public partial class RecipePage : ContentPage
{
    public RecipePage(RecipePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}