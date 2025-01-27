using System.Collections.Generic;
using System.Threading.Tasks;
using Meadow.Foundation.Controllers.Pid;
using Meadow.Hardware;

namespace BrewIoT.Device.Meadow.Controllers;

public class SlideContents
{
    private readonly StandardPidController pidController;
    private float targetTemperature;
    public float LiquidTemperature { get; private set; }


    public SlideContents()
    {


        pidController = new StandardPidController();
        pidController.ProportionalComponent = .5f; // proportional
        pidController.IntegralComponent = .55f; // integral time minutes
        pidController.DerivativeComponent = 0f; // derivative time in minutes
        pidController.OutputMin = -1.0f; // -100% power minimum or 100% cooling
        pidController.OutputMax = 1.0f; // 100% power max
        pidController.OutputTuningInformation = true;
    }

    public Task Read()
    {
        // set our input and target on the PID calculator
        pidController.ActualInput = (float)LiquidTemperature;
        pidController.TargetInput = (float)this.targetTemperature;

        // get the appropriate power level
        var powerLevel = pidController.CalculateControlOutput();


        return Task.CompletedTask;
    }
}

public class PwmSlide
{
    private SoftPwmPort heaterRelayPwm;
    private readonly StandardPidController pidController;

    public void Initialize()
    {



        heaterRelayPwm = new SoftPwmPort(MeadowApp.Device.Pins.D02, 1, 0.1f);
    }

    public void Write()
    {
        var powerLevel = pidController.CalculateControlOutput();

        heaterRelayPwm.DutyCycle = powerLevel;

        
    }
}