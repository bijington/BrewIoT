using Meadow;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Peripherals.Sensors;

namespace BrewIoT.Device.Meadow.Slides;

public class LiquidTemperatureMonitor
{
    private ITemperatureSensor liquidTemperatureSensor;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public void Initialize()
    {
        liquidTemperatureSensor = new AnalogTemperature(
            analogPin: MeadowApp.Device.Pins.A00,
            sensorType: AnalogTemperature.KnownSensorType.LM35
        );   

        Resolver.SensorService.RegisterSensor(liquidTemperatureSensor);

        liquidTemperatureSensor.StartUpdating();
    }

    public void Read()
    {
        LiquidTemperature = liquidTemperatureSensor.Temperature?.Celsius ?? double.NaN;
    }
}