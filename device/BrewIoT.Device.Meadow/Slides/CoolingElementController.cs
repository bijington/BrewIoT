using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;

namespace BrewIoT.Device.Meadow.Slides;

public class CoolingElementController
{
    private Relay coolingRelay;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public static double TargetTemperature { get; private set; } = double.NaN;

    public void Initialize()
    {
        coolingRelay = new Relay(MeadowApp.Device.Pins.D03);
    }

    public void Write()
    {
        if (LiquidTemperature <= TargetTemperature)
        {
            coolingRelay.State = RelayState.Open;
        }
        else
        {
            coolingRelay.State = RelayState.Closed;
        }
    }
}