using BrewIoT.Device.Pi.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrewIoT.Device.Pi;

public class Program
{
    private static async Task Main(string[] args)
    {
        var appBuilder = Host.CreateApplicationBuilder(args);

        // appBuilder.Services.AddHttpClient<ReadingsApiClient>();
        // appBuilder.Services.AddSingleton<ReadingsApiClient>();
        appBuilder.Services.AddSingleton<TemperatureController>();
        
        var host = appBuilder.Build();
        
        var application = host.Services.GetRequiredService<PiApplication>();

        await application.Run(CancellationToken.None);
    }
}
