using System;
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
        var currentJobStage = JobController.CurrentJobStage;

        displayView.WriteLine(currentJobStage?.Name, 0);
        displayView.WriteLine($"Target: {currentJobStage?.TargetTemperature:0.0}", 1);
        displayView.WriteLine($"Liquid: {TemperatureController.LiquidTemperature:0.0}", 2);
        var action = TemperatureController.PowerLevel > 0 ? "Heating" : "Cooling";
        displayView.WriteLine($"{action}: {Math.Abs(TemperatureController.PowerLevel) * 100d:0}%", 3);
    }
}