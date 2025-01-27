using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;

namespace BrewIoT.Device.Meadow.Slides;

public class HeatingElementController
{
    private Relay heatingRelay;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public static double TargetTemperature { get; private set; } = double.NaN;

    public void Initialize()
    {
        heatingRelay = new Relay(MeadowApp.Device.Pins.D02);
    }

    public void Write()
    {
        if (LiquidTemperature < TargetTemperature)
        {
            heatingRelay.State = RelayState.Closed;
        }
        else
        {
            heatingRelay.State = RelayState.Open;
        }
    }
}