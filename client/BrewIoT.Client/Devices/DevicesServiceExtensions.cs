using BrewIoT.Client.Devices.Pages;
using BrewIoT.Client.Devices.ViewModels;
using Refit;

namespace BrewIoT.Client.Devices;

public static class DevicesServiceExtensions
{
    public static IServiceCollection AddDevices(this IServiceCollection services)
    {
        services.AddRefitClient<IDeviceApiService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7207"));
        
        Routing.RegisterRoute(nameof(DevicePage), typeof(DevicePage));
        
        return services
            .AddTransient<DeviceListPage>()
            .AddTransient<DevicePage>()
            .AddTransient<DeviceListPageViewModel>()
            .AddTransient<DevicePageViewModel>();
    }
}