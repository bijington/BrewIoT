using BrewIoT.Client.Devices.Pages;
using BrewIoT.Client.Devices.ViewModels;
using Refit;

namespace BrewIoT.Client.Devices;

public static class DevicesServiceExtensions
{
    public static IServiceCollection AddDevices(this IServiceCollection services)
    {
        services.AddRefitClient<IDeviceApiService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7466"));
        
        return services
            .AddTransient<DeviceListPage>()
            .AddTransient<DeviceListPageViewModel>();
    }
}