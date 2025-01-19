using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewIot.Meadow;

public class MeadowApp : App<F7FeatherV2>
{
    readonly IReadOnlyList<IController> controllers;
    DisplayView displayView;

    public MeadowApp()
    {
        controllers = new List<IController>
        {
            // new JobController(),
            new TemperatureController(Device, Device.Pins)
        };
    }

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        displayView = new DisplayView();

        foreach (var controller in controllers)
        {
            controller.Initialize(Settings);
        }

        return base.Initialize();
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        displayView.WriteLine($"Running app", 0);

        foreach (var controller in controllers)
        {
            controller.Run();
        }

        return UpdateDisplay();
    }

    async Task UpdateDisplay()
    {
        Resolver.Log.Info("Cycle colors...");

        while (true)
        {
            var currentJobStage = JobController.GetCurrentJobStage();

            displayView.WriteLine(currentJobStage.Name, 0);
            displayView.WriteLine($"Ambient: {TemperatureController.AmbientTemperature}", 1);
            displayView.WriteLine($"Liquid: {TemperatureController.LiquidTemperature}", 2);
            displayView.WriteLine($"{TemperatureController.HeatingMode} to: {currentJobStage.TargetTemperature}", 3);

            await Task.Delay(1000);
        }
    }
}