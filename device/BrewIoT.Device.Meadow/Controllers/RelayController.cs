using System.Collections.Generic;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;

namespace BrewIoT.Device.Meadow.Controllers;

// Put enough logic into the IoT device to make it safely handle scenarios but not so much that it becomes a monolith.
// Put the control logic into the next level up.

public class RelayController : IController
{
    private Relay relay;
    private Relay relay2;

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        relay = new Relay(MeadowApp.Device.Pins.D02);
        relay2 = new Relay(MeadowApp.Device.Pins.D03);
    }

    private double LiquidTemperature {get;set;}
    private double TargetTemperature {get;set;}

    public Task Read()
    {
        return Task.CompletedTask;
    }

    private void Jeff()
    {


        relay = new Relay(MeadowApp.Device.Pins.D03);
    }

    public void Write()
    {
        if (LiquidTemperature > TargetTemperature)
        {
            relay.State = RelayState.Closed;
        }
        else
        {
            relay.State = RelayState.Open;
        }


        relay2.State = relay.State == RelayState.Closed ? RelayState.Open : RelayState.Closed;

        Resolver.Log.Info($"Relay state: {relay.State} -> relay2 state: {relay2.State}");
    }
}