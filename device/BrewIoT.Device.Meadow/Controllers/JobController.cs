using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewIoT.Device.Meadow;

public class JobController : IController
{
    public static JobStage CurrentJobStage { get; private set; }
    private string url;

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        url = settings["Job.Url"];
        DeviceApiService.Initialize(url);

        CurrentJobStage = new JobStage
        {
            Name = "Fermentation",
            TargetTemperature = 20
        };
    }

    public Task Read()
    {
        //CurrentJobStage = await DeviceApiService.GetCurrentJobStage(url);
        return Task.CompletedTask;
    }

    public void Write()
    {
    }
}