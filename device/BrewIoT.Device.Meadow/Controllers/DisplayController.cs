using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewIoT.Device.Meadow.Controllers;

public class DisplayController : IController
{
    DisplayView displayView;

    public DisplayController()
    {
        displayView = new ();
    }

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        displayView.WriteLine($"Initializing", 0);
    }

    public Task Read() => Task.CompletedTask;

    public void Write()
    {
        var currentJobStage = JobController.GetCurrentJobStage();

        displayView.WriteLine(currentJobStage.Name, 0);
        displayView.WriteLine($"Ambient: {TemperatureController.AmbientTemperature}", 1);
        displayView.WriteLine($"Liquid: {TemperatureController.LiquidTemperature}", 2);
        displayView.WriteLine($"{TemperatureController.HeatingMode} to: {currentJobStage.TargetTemperature}", 3);
    }
}