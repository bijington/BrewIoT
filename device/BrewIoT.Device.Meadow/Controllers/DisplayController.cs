using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meadow;

namespace BrewIoT.Device.Meadow.Controllers;

public class DisplayController : IController
{
    private readonly DisplayView displayView = new ();

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

        string action = "Off";

        if (TemperatureController.LiquidTemperature < TemperatureController.TargetTemperature)
        {
            action = "Heating";
        }
        else if (TemperatureController.LiquidTemperature > TemperatureController.TargetTemperature)
        {
            action = "Cooling";
        }

        displayView.WriteLine($"{action}: {Math.Abs(TemperatureController.PowerLevel) * 100d:0}%", 3);

        Resolver.Log.Info(currentJobStage?.Name);
        Resolver.Log.Info($"Target: {currentJobStage?.TargetTemperature:0.0}");
        Resolver.Log.Info($"Liquid: {TemperatureController.LiquidTemperature:0.0}");
        Resolver.Log.Info($"{action}: {Math.Abs(TemperatureController.PowerLevel) * 100d:0}%");
    }
}