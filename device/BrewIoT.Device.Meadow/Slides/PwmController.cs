using Meadow.Foundation.Controllers.Pid;
using Meadow.Hardware;

namespace BrewIoT.Device.Meadow.Slides;

public class PwmController
{
    private SoftPwmPort heaterRelayPwm;
    private readonly StandardPidController pidController = new StandardPidController();

    public void Initialize()
    {
        heaterRelayPwm = new SoftPwmPort(MeadowApp.Device.Pins.D02, dutyCycle: 1, frequency: 0.1f);
    }

    public void Write()
    {
        var powerLevel = pidController.CalculateControlOutput();

        heaterRelayPwm.DutyCycle = powerLevel;
    }
}