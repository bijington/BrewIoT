using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using Meadow.Hardware;
using System.Linq;
using System.Collections.Generic;
using Meadow.Foundation.Sensors;

namespace BrewIoT.Device.Meadow;

// Put enough logic into the IoT device to make it safely handle scenarios but not so much that it becomes a monolith.
// Put the control logic into the next level up.

public class JobController : IController
{
    private static JobStage currentJobStage;

    public async void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        // currentJobStage = await DeviceApiService.GetCurrentJobStage();
    }

    public void Run()
    {
        
    }

    public static JobStage GetCurrentJobStage()
    {
        return currentJobStage ??= new JobStage
        {
            Name = "Fermentation",
            TargetTemperature = 24,
            HeatingMode = HeatingMode.Heating
        };
    }
}