using System.Threading.Tasks;
using BrewIoT.Device.Meadow.Sensors;

namespace BrewIoT.Device.Meadow.Slides;

public class LiquidTemperatureMonitor
{
    private Max31865TemperatureSensor liquidTemperatureSensor;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public void Initialize()
    {
        liquidTemperatureSensor = new Max31865TemperatureSensor(
            MeadowApp.Device.CreateSpiBus(),
            MeadowApp.Device.CreateDigitalOutputPort(MeadowApp.Device.Pins.D03),
            Max31865TemperatureSensor.KnownSensorType.PT100
        );
    }

    public async Task Read()
    {
        var temperature = await liquidTemperatureSensor.Read();

        LiquidTemperature = temperature.Celsius;
    }
}