using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BrewIoT.Client.Devices.ViewModels;

public partial class DevicePageViewModel : ObservableObject, IQueryAttributable
{
    private readonly IDeviceApiService deviceApiService;

    [ObservableProperty] 
    private string name = string.Empty;
    
    [ObservableProperty] 
    private ObservableCollection<DeviceReading> readings = [];
    
    [ObservableProperty] 
    private string noReadingsMessage = "No readings available.";

    public DevicePageViewModel(IDeviceApiService deviceApiService)
    {
        this.deviceApiService = deviceApiService;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            var device = (Device)query["Device"];
            
            Name = device.Name;
            
            var deviceReadings = await deviceApiService.GetDeviceReadings(device.Id);
            
            Readings = new ObservableCollection<DeviceReading>(deviceReadings);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoReadingsMessage = e.Message;
            throw;
        }
    }
}