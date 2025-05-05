using Meadow.Foundation.Controllers.Pid;

namespace BrewIoT.Device.Meadow.Slides;

public class PidCalculationController
{
    private StandardPidController pidController;

    public static float LiquidTemperature { get; private set; } = float.NaN;

    public static float TargetTemperature { get; private set; } = float.NaN;

    public PidCalculationController()
    {
        pidController = new StandardPidController();
        pidController.ProportionalComponent = .5f; // proportional
        pidController.IntegralComponent = .55f; // integral time minutes
        pidController.DerivativeComponent = 0f; // derivative time in minutes
        pidController.OutputMin = 0f; // 0% power minimum
        pidController.OutputMax = 1.0f; // 100% power max
        pidController.OutputTuningInformation = true;
    }

    public void Read()
    {
        // set our input and target on the PID calculator
        pidController.ActualInput = LiquidTemperature;
        pidController.TargetInput = TargetTemperature;

        var powerLevel = pidController.CalculateControlOutput();
    }
}