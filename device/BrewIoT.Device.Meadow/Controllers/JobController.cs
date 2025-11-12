using System.Collections.Generic;
using System.Threading.Tasks;
using System;
//using BrewIoT.Device.Api;

namespace BrewIoT.Device.Meadow.Controllers;

public class JobController : IController
{
    public static JobStage CurrentJobStage { get; private set; }
    private string url;

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        // url = settings["Job.Url"];
        // DeviceApiService.Initialize(url);

        CurrentJobStage = new JobStage
        {
            Name = "Cold crash",
            TargetTemperature = 2
        };
    }

    public async Task Read()
    {
        //CurrentJobStage = await DeviceApiService.GetCurrentJobStage();
        await Task.Delay(1000); // Simulate async operation
    }

    public void Write()
    {
    }
}

public sealed class JobStage
    {
        public string Name { get; set; } = string.Empty;

        public double TargetTemperature { get; set; }
    }