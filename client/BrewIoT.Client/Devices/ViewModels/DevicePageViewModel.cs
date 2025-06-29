using System.Collections.ObjectModel;
using BrewIoT.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BrewIoT.Client.Devices.ViewModels;

public partial class DevicePageViewModel : ObservableObject, IQueryAttributable
{
    private readonly IDeviceApiService deviceApiService;
    private readonly IDispatcherTimer dispatcherTimer;
    
    private Device? device;

    [ObservableProperty]
    private string name = string.Empty;
    
    [ObservableProperty]
    private DateTime lastUpdated = DateTime.MinValue;
    
    [ObservableProperty] 
    private ObservableCollection<DeviceReading> readings = [];
    
    [ObservableProperty] 
    private string noReadingsMessage = "No readings available.";

    public DevicePageViewModel(IDeviceApiService deviceApiService, IDispatcher dispatcher)
    {
        this.deviceApiService = deviceApiService;

        this.dispatcherTimer = dispatcher.CreateTimer();
        this.dispatcherTimer.IsRepeating = false;
        this.dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
        this.dispatcherTimer.Tick += DispatcherTimerOnTick;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            device = (Device)query["Device"];
            
            Name = device.Name;
            
            await LoadReadings();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoReadingsMessage = e.Message;
        }
    }

    public void OnNavigatingFrom()
    {
        this.dispatcherTimer.Stop();
        this.dispatcherTimer.Tick -= DispatcherTimerOnTick;
    }
    
    private async void DispatcherTimerOnTick(object? sender, EventArgs e)
    {
        await LoadReadings();
    }

    private async Task LoadReadings()
    {
        if (device is null)
        {
            return;
        }
        
        try
        {
            var deviceReadings = await deviceApiService.GetDeviceReadings(device.Id);
            
            Readings = new ObservableCollection<DeviceReading>(deviceReadings.OrderByDescending(x => x.Timestamp));
            
            LastUpdated = deviceReadings.LastOrDefault()?.Timestamp ?? DateTime.MinValue;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NoReadingsMessage = e.Message;
            throw;
        }
        
        this.dispatcherTimer.Start();
    }
}