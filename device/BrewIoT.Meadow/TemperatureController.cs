using Meadow;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using Meadow.Hardware;
using System.Collections.Generic;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Temperature;
using System;
using Meadow.Peripherals.Relays;

namespace BrewIot.Meadow;

// Put enough logic into the IoT device to make it safely handle scenarios but not so much that it becomes a monolith.
// Put the control logic into the next level up.

public class TemperatureController : IController
{
    private readonly IDigitalInputOutputController device;
    private readonly IPinDefinitions pins;
    private ITemperatureSensor liquidTemperatureSensor;
    private ITemperatureSensor ambientTemperatureSensor;
    private Relay heatingRelay;
    private Relay coolingRelay;

    public static double LiquidTemperature { get; private set; }

    public static double AmbientTemperature { get; private set; }

    public static double InternalAirTemperature { get; private set; }

    public static HeatingMode HeatingMode { get; private set; }

    public TemperatureController(IDigitalInputOutputController device, IPinDefinitions pins)
    {
        this.device = device;
        this.pins = pins;
    }

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        bool.TryParse(settings["Temperature.IsSimulated"], out bool isSimulated);

        Resolver.Log.Info("TemperatureController Initialize...");

        if (isSimulated)
        {
            Resolver.Log.Info("TemperatureController running in simulated mode");

            liquidTemperatureSensor = new SimulatedTemperatureSensor(
                new Temperature(15, Temperature.UnitType.Celsius),
                new Temperature(10, Temperature.UnitType.Celsius),
                new Temperature(25, Temperature.UnitType.Celsius),
                SimulationBehavior.Sine);

            ambientTemperatureSensor = new SimulatedTemperatureSensor(
                new Temperature(3, Temperature.UnitType.Celsius),
                new Temperature(-2, Temperature.UnitType.Celsius),
                new Temperature(6, Temperature.UnitType.Celsius),
                SimulationBehavior.Sine);
        }
        //else
        {
            // var analogPin = pins.FirstOrDefault(p => p.Name == "A0e");
            // heatingRelay = new Relay(device.(digitalOutput2));

            // configure our AnalogTemperature sensor
            ambientTemperatureSensor = new AnalogTemperature(
                analogPin: MeadowApp.Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );

            // var analogInput = MeadowApp.Device.CreateAnalogInputPort(MeadowApp.Device.Pins.A01, 10, TimeSpan.FromSeconds(1), new Voltage(5));
            // analogInput.Updated += (s, e) =>
            // {
            //     var voltage = e.New;
            //     Resolver.Log.Info($"Voltage: {voltage.Millivolts}mv");
            //     LiquidTemperature = voltage.Millivolts;
            // };

            // analogInput.StartUpdating();
            
            liquidTemperatureSensor = new AnalogTemperature(
                analogPin: MeadowApp.Device.Pins.A01,
                sensorType: AnalogTemperature.KnownSensorType.LM35,
                null,
                10,
                TimeSpan.FromSeconds(1)
                //new AnalogTemperature.Calibration(21.5, 1.4, 0.0142)
                // 21.5 -> 1.4
                // 10f -> 1.25
            );
        }

        liquidTemperatureSensor.Updated += (s, e) =>
        {
            OnTemperatureChanged(e);
        };

        ambientTemperatureSensor.Updated += (s, e) =>
        {
            OnAmbientTemperatureChanged(e);
        };

        // //==== IObservable Pattern with an optional notification filter.
        // var consumer = AnalogTemperature.CreateObserver(
        //     handler: result => OnTemperatureChanged(result),

        //     // only notify if the change is greater than 0.5°C
        //     filter: result =>
        //     {
        //         if (result.Old is { } old)
        //         {
        //             return (result.New - old).Abs().Celsius > 0.5; // returns true if > 0.5°C change.
        //         }
        //         return false;
        //     }
        //     // if you want to always get notified, pass null for the filter:
        //     //filter: null
        // );
        // analogTemperature.Subscribe(consumer);

        // Resolver.SensorService.RegisterSensor(ambientTemperatureSensor);
        // Resolver.SensorService.RegisterSensor(internalAirTemperatureSensor);
        // Resolver.SensorService.RegisterSensor(liquidTemperatureSensor);

        ambientTemperatureSensor.StartUpdating();
        liquidTemperatureSensor.StartUpdating();

        heatingRelay = new Relay(MeadowApp.Device.Pins.D02);
        coolingRelay = new Relay(MeadowApp.Device.Pins.D03);
    }

    public void Run()
    {
        Resolver.Log.Info("TemperatureController Run...");
    }

    private void OnAmbientTemperatureChanged(IChangeResult<Temperature> e)
    {
        //Resolver.Log.Info($"Ambient temperature changed: {e.New.Celsius:N2}C, old: {e.Old?.Celsius:N2}C");

        AmbientTemperature = e.New.Celsius;
    }

    private void OnTemperatureChanged(IChangeResult<Temperature> e)
    {
        Resolver.Log.Info($"Temperature changed: {e.New.Celsius:N2}C, old: {e.Old?.Celsius:N2}C");

        LiquidTemperature = e.New.Celsius;

        // Heating modes
        var currentJobStage = JobController.GetCurrentJobStage();
        var action = HeatingMode.Off;
        var heatingMode = HeatingMode.Off;

        if (AmbientTemperature < currentJobStage.TargetTemperature)
        {
            heatingMode = HeatingMode.Heating;
        }
        else if (AmbientTemperature > currentJobStage.TargetTemperature)
        {
            heatingMode = HeatingMode.Cooling;
        }

        // A further optimization would be to control the power of the heater.
        // We could also remove the need for HeatingMode property and have an external sensor to compare to the air temperature.
        switch (heatingMode)
        {
            case HeatingMode.Off:
                heatingRelay.State = RelayState.Closed;
                coolingRelay.State = RelayState.Closed;
                break;

            case HeatingMode.Heating:
                coolingRelay.State = RelayState.Closed;

                // If we are traveling towards the target temperature, turn on the heating relay, if we overshoot just turn if off
                if (e.New.Celsius >= currentJobStage.TargetTemperature)
                {
                    heatingRelay.State = RelayState.Closed;

                    // If we have somehow massively overshot then we could cool.
                }
                else
                {
                    heatingRelay.State = RelayState.Open;
                    action = HeatingMode.Heating;
                }
                break;

            case HeatingMode.Cooling:
                heatingRelay.State = RelayState.Closed;

                // If we are traveling towards the target temperature, turn on the cooling relay, if we overshoot just turn if off
                if (e.New.Celsius <= currentJobStage.TargetTemperature)
                {
                    coolingRelay.State = RelayState.Closed;

                    // If we have somehow massively overshot then we could heat.
                }
                else
                {
                    coolingRelay.State = RelayState.Open;
                    action = HeatingMode.Cooling;
                }
                break;
        }

        HeatingMode = action;

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