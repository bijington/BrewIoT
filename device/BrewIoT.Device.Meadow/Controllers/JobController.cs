using System.Collections.Generic;
using System.Threading.Tasks;
using BrewIoT.Device.Api;

namespace BrewIoT.Device.Meadow.Controllers;

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
            TargetTemperature = 26
        };
    }

    public async Task Read()
    {
        CurrentJobStage = await DeviceApiService.GetCurrentJobStage();
    }

    public void Write()
    {
    }
}