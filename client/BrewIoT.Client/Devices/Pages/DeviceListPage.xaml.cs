using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrewIoT.Client.Devices.ViewModels;

namespace BrewIoT.Client.Devices.Pages;

public partial class DeviceListPage : ContentPage
{
    private readonly DeviceListPageViewModel viewModel;
    
    public DeviceListPage(DeviceListPageViewModel viewModel)
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