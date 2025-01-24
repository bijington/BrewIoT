using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewIoT.Device.Meadow;

public class JobController : IController
{
    private static JobStage currentJobStage;

    public void Initialize(IReadOnlyDictionary<string, string> settings)
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

    public Task Read()
    {
        //currentJobStage = await DeviceApiService.GetCurrentJobStage();
        return Task.CompletedTask;
    }

    public void Write()
    {
    }
}