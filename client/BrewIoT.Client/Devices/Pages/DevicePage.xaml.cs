using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrewIoT.Client.Devices.ViewModels;

namespace BrewIoT.Client.Devices.Pages;

public partial class DevicePage : ContentPage
{
    public DevicePage(DevicePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}