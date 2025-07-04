﻿using BrewIoT.Device.Meadow.Controllers;
using Meadow;
using Meadow.Devices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewIoT.Device.Meadow;

public class MeadowApp : App<F7FeatherV2>
{
    readonly IReadOnlyList<IController> controllers =
    [
        new JobController(),
        new TemperatureController(),
        new DisplayController()
    ];

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        foreach (var controller in controllers)
        {
            controller.Initialize(Settings);
        }

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        while (true)
        {
            foreach (var controller in controllers)
            {
                await controller.Read();
            }

            foreach (var controller in controllers)
            {
                controller.Write();
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}