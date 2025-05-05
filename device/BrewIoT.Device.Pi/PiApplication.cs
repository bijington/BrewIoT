using BrewIoT.Device.Pi.Controllers;

namespace BrewIoT.Device.Pi;

public class PiApplication
{
    private readonly IReadOnlyList<IController> controllers;
    
    public PiApplication(TemperatureController temperatureController)
    {
        controllers = [temperatureController];
    }
    
    public async Task Run(CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested is false)
        {
            foreach (var controller in controllers)
            {
                controller.Read();
            }

            foreach (var controller in controllers)
            {
                controller.Write();
            }

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
}