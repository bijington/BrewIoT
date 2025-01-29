using Meadow;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Peripherals.Sensors;

namespace BrewIoT.Device.Meadow.Slides;

public class AmbientTemperatureMonitor
{
    private ITemperatureSensor ambientTemperatureSensor;

    public static double AmbientTemperature { get; private set; } = double.NaN;

    public void Initialize()
    {
        ambientTemperatureSensor = new AnalogTemperature(
            analogPin: MeadowApp.Device.Pins.A01,
            sensorType: AnalogTemperature.KnownSensorType.LM35
        );

        Resolver.SensorService.RegisterSensor(ambientTemperatureSensor);

        ambientTemperatureSensor.StartUpdating();
    }

    public void Read()
    {
        AmbientTemperature = ambientTemperatureSensor.Temperature?.Celsius ?? double.NaN;
    }
}