using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using Meadow.Hardware;
using System.Linq;
using System.Collections.Generic;
using Meadow.Foundation.Sensors;
using System.IO;
using Meadow.Foundation.Sensors.Temperature;
using System;
using Meadow.Foundation.Controllers.Pid;

namespace BrewIoT.Device.Meadow;

// Put enough logic into the IoT device to make it safely handle scenarios but not so much that it becomes a monolith.
// Put the control logic into the next level up.

public class PidController : PidControllerBase
{
    public override float CalculateControlOutput()
    {
        throw new NotImplementedException();
    }
}