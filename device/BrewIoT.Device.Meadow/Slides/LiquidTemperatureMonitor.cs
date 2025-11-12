using System.Threading.Tasks;
using BrewIoT.Device.Meadow.Sensors;

namespace BrewIoT.Device.Meadow.Slides;

public class LiquidTemperatureMonitor
{
    private Max31865 liquidTemperatureSensor;

    public static double LiquidTemperature { get; private set; } = double.NaN;

    public void Initialize()
    {
        liquidTemperatureSensor = new Max31865(
            MeadowApp.Device.CreateSpiBus(),
            MeadowApp.Device.CreateDigitalOutputPort(MeadowApp.Device.Pins.D03),
            Max31865.KnownSensorType.PT100,
            Max31865.Wires.ThreeWire,
            100,
            Max31865.ConversionFilterMode.Filter50Hz
        );
    }

    public async Task Read()
    {
        var temperature = await liquidTemperatureSensor.Read();

        LiquidTemperature = temperature.Celsius;
    }
}