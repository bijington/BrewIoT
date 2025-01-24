using Meadow;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System.Collections.Generic;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Temperature;
using System.Threading.Tasks;
using Meadow.Foundation.Controllers.Pid;
using Meadow.Hardware;

namespace BrewIoT.Device.Meadow;

// Put enough logic into the IoT device to make it safely handle scenarios but not so much that it becomes a monolith.
// Put the control logic into the next level up.

public class TemperatureController : IController
{
    private ITemperatureSensor liquidTemperatureSensor;
    private ITemperatureSensor ambientTemperatureSensor;
    private SoftPwmPort coolingRelayPwm;
    private SoftPwmPort heaterRelayPwm;
    private StandardPidController pidController;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public static double AmbientTemperature { get; private set; } = double.NaN;

    public static HeatingMode HeatingMode { get; private set; }

    private double targetTemperature;

    public TemperatureController()
    {
        pidController = new StandardPidController();
        pidController.ProportionalComponent = .5f; // proportional
        pidController.IntegralComponent = .55f; // integral time minutes
        pidController.DerivativeComponent = 0f; // derivative time in minutes
        pidController.OutputMin = 0.0f; // 0% power minimum
        pidController.OutputMax = 1.0f; // 100% power max
        pidController.OutputTuningInformation = true;
    }

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        bool.TryParse(settings["Temperature.IsSimulated"], out bool isSimulated);

        if (isSimulated)
        {
            Resolver.Log.Info("TemperatureController running in simulated mode");

            ambientTemperatureSensor = new SimulatedTemperatureSensor(
                new Temperature(3, Temperature.UnitType.Celsius),
                new Temperature(-2, Temperature.UnitType.Celsius),
                new Temperature(6, Temperature.UnitType.Celsius),
                SimulationBehavior.Sine);

            liquidTemperatureSensor = new SimulatedTemperatureSensor(
                new Temperature(15, Temperature.UnitType.Celsius),
                new Temperature(10, Temperature.UnitType.Celsius),
                new Temperature(25, Temperature.UnitType.Celsius),
                SimulationBehavior.Sine);
        }
        else
        {
            ambientTemperatureSensor = new AnalogTemperature(
                analogPin: MeadowApp.Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );

            liquidTemperatureSensor = new AnalogTemperature(
                analogPin: MeadowApp.Device.Pins.A01,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );
        }

        Resolver.SensorService.RegisterSensor(ambientTemperatureSensor);
        Resolver.SensorService.RegisterSensor(liquidTemperatureSensor);

        // ambientTemperatureSensor.StartUpdating();
        liquidTemperatureSensor.StartUpdating();

        // Initialize in off state.
        heaterRelayPwm = new SoftPwmPort(MeadowApp.Device.Pins.D02, 1, 0.1f);
        coolingRelayPwm = new SoftPwmPort(MeadowApp.Device.Pins.D03, 1, 0.1f);

        heaterRelayPwm.Inverted = true;
        coolingRelayPwm.Inverted = true;
    }

    public Task Read()
    {
        AmbientTemperature = ambientTemperatureSensor.Temperature?.Celsius ?? double.NaN;
        LiquidTemperature = liquidTemperatureSensor.Temperature?.Celsius ?? double.NaN;

        var currentJobStage = JobController.CurrentJobStage;
        targetTemperature = currentJobStage.TargetTemperature;

        HeatingMode = HeatingMode.Off;

        if (AmbientTemperature < currentJobStage.TargetTemperature)
        {
            HeatingMode = HeatingMode.Heating;
        }
        else if (AmbientTemperature > currentJobStage.TargetTemperature)
        {
            HeatingMode = HeatingMode.Cooling;
        }

        return Task.CompletedTask;
    }

    public void Write()
    {
        if (liquidTemperatureSensor.Temperature is null)
        {
            return;
        }

        ReportReadings();

        // set our input and target on the PID calculator
        pidController.ActualInput = (float)liquidTemperatureSensor.Temperature?.Celsius;
        pidController.TargetInput = (float)this.targetTemperature;

        // get the appropriate power level
        var powerLevel = pidController.CalculateControlOutput();

        switch (HeatingMode)
        {
            case HeatingMode.Off:
                SetRelayPower(heaterRelayPwm, 0);
                SetRelayPower(coolingRelayPwm, 0);
                break;

            case HeatingMode.Heating:
                SetRelayPower(heaterRelayPwm, powerLevel);
                SetRelayPower(coolingRelayPwm, 0);
                break;

            case HeatingMode.Cooling:
                SetRelayPower(heaterRelayPwm, 0);
                SetRelayPower(coolingRelayPwm, powerLevel);
                break;
        }
    }

    private void SetRelayPower(SoftPwmPort softPwmPort, double powerLevel)
    {
        if (powerLevel == 0)
        {
            softPwmPort.Stop();
        }
        else
        {
            softPwmPort.Start();
        }

        softPwmPort.DutyCycle = powerLevel;
    }

    private void ReportReadings()
    {
        // _ = DeviceApiService.ReportReadings(
        //     1,
        //     new Readings
        //     {
        //         LiquidTemperature = e.New.Celsius,
        //         AmbientTemperature = AmbientTemperature,
        //         InternalAirTemperature = InternalAirTemperature,
        //         Action = action
        //     });
    }
}
