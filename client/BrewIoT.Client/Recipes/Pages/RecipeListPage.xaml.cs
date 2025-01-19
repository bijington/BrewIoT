using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrewIoT.Client.Recipes.ViewModels;

namespace BrewIoT.Client.Recipes.Pages;

public partial class RecipeListPage : ContentPage
{
    private readonly RecipeListPageViewModel viewModel;
    
    public RecipeListPage(RecipeListPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        viewModel.OnNavigatedTo();
    }
}