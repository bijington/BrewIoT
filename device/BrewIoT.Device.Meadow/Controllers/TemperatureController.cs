using Meadow;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System.Collections.Generic;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Temperature;
using System.Threading.Tasks;
using Meadow.Foundation.Controllers.Pid;
using Meadow.Hardware;
using System;

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
    private double powerLevel;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public static double AmbientTemperature { get; private set; } = double.NaN;

    public static double PowerLevel { get; private set; }

    private double targetTemperature;

    public TemperatureController()
    {
        pidController = new StandardPidController();
        pidController.ProportionalComponent = .5f; // proportional
        pidController.IntegralComponent = .55f; // integral time minutes
        pidController.DerivativeComponent = 0f; // derivative time in minutes
        pidController.OutputMin = -1.0f; // 0% power minimum
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

            ((SimulatedTemperatureSensor)ambientTemperatureSensor).StartSimulation(SimulationBehavior.Sine);

            liquidTemperatureSensor = new SimulatedTemperatureSensor(
                new Temperature(19.5, Temperature.UnitType.Celsius),
                new Temperature(19, Temperature.UnitType.Celsius),
                new Temperature(21, Temperature.UnitType.Celsius),
                SimulationBehavior.Sine);

            ((SimulatedTemperatureSensor)liquidTemperatureSensor).StartSimulation(SimulationBehavior.Sine);
        }
        else
        {
            ambientTemperatureSensor = new AnalogTemperature(
                MeadowApp.Device.CreateAnalogInputPort(MeadowApp.Device.Pins.A00, 10, System.TimeSpan.FromMilliseconds(40), new Voltage(5)),
                sensorType: AnalogTemperature.KnownSensorType.LM35//,
                //new AnalogTemperature.Calibration(19.0, 384.615384615383, 15.0)
            );

            liquidTemperatureSensor = new AnalogTemperature(
                analogPin: MeadowApp.Device.Pins.A01,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );
        }

        // var analogInput = MeadowApp.Device.CreateAnalogInputPort(MeadowApp.Device.Pins.A00, 10, System.TimeSpan.FromSeconds(1), new Voltage(5));
        //     analogInput.Updated += (s, e) =>
        //     {
        //         var voltage = e.New;
        //         Resolver.Log.Info($"Voltage: {voltage.Millivolts}mv");
        //         AmbientTemperature = voltage.Millivolts;
        //     };

        //     analogInput.StartUpdating();

        Resolver.SensorService.RegisterSensor(ambientTemperatureSensor);
        Resolver.SensorService.RegisterSensor(liquidTemperatureSensor);

        ambientTemperatureSensor.StartUpdating();
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

        // set our input and target on the PID calculator
        pidController.ActualInput = (float)liquidTemperatureSensor.Temperature?.Celsius;
        pidController.TargetInput = (float)this.targetTemperature;

        // get the appropriate power level
        powerLevel = pidController.CalculateControlOutput();
        PowerLevel = powerLevel;

        Resolver.Log.Info($"Power level: {powerLevel}");

        return Task.CompletedTask;
    }

    public void Write()
    {
        if (liquidTemperatureSensor.Temperature is null)
        {
            return;
        }

        ReportReadings();

        SetRelayPower(heaterRelayPwm, Math.Clamp(powerLevel, 0, 1));
        SetRelayPower(coolingRelayPwm, Math.Abs(Math.Clamp(powerLevel, -1, 0)));
    }

    private void SetRelayPower(SoftPwmPort softPwmPort, double powerLevel)
    {
        // if (powerLevel == 0)
        // {
        //     //softPwmPort.Stop();
        // }
        // else
        if (softPwmPort.State is false)
        {
            softPwmPort.Start();
        }

        softPwmPort.DutyCycle = powerLevel;

        Resolver.Log.Info($"Relay power: {powerLevel} for {softPwmPort.Pin.Name} Target: {targetTemperature} Liquid: {LiquidTemperature} Ambient: {AmbientTemperature}");
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
