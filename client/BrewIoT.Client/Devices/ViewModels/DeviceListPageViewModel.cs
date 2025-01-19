using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BrewIoT.Client.Devices.ViewModels;

public partial class DeviceListPageViewModel : ObservableObject
{
    private readonly IDeviceApiService deviceApiService;

    [ObservableProperty] 
    private ObservableCollection<Device> devices = [];
    
    [ObservableProperty] 
    private string noDevicesMessage = "No devices available.";

    public DeviceListPageViewModel(IDeviceApiService deviceApiService)
    {
        this.deviceApiService = deviceApiService;
    }

    public async void OnNavigatedTo()
    {
        try
        {
            var connectedDevices = await deviceApiService.GetDevices();
            
            Devices = new ObservableCollection<Device>(connectedDevices);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoDevicesMessage = e.Message;
            throw;
        }
        
    }
}