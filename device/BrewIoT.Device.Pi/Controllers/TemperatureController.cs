using System.Device.Spi;
using Iot.Device.Max31865;
using UnitsNet;

namespace BrewIoT.Device.Pi.Controllers;

public class TemperatureController : IController
{
    private readonly ResistanceTemperatureDetectorWires resistanceTemperatureDetectorWires = ResistanceTemperatureDetectorWires.ThreeWire;
    private readonly int chipSelectLine = 0;
    
    public void Read()
    {
        SpiConnectionSettings settings = new(0, chipSelectLine)
        {
            ClockFrequency = Max31865.SpiClockFrequency,
            Mode = Max31865.SpiMode1,
            DataFlow = Max31865.SpiDataFlow
        };

        Console.WriteLine($"Wire: {resistanceTemperatureDetectorWires}");
        
        using SpiDevice device = SpiDevice.Create(settings);
        //using Max31865 sensor = new(device, PlatinumResistanceThermometerType.Pt100, resistanceTemperatureDetectorWires, ElectricResistance.FromOhms(430), ConversionFilterMode);
        
        //return sensor.Temperature.DegreesCelsius;
    }

    public void Write()
    {
        // if (liquidTemperatureSensor.Temperature is null)
        // {
        //     return;
        // }
        //
        // ReportReadings();
        //
        // if (LiquidTemperature < TargetTemperature)
        // {
        //     SetRelayPower(heaterRelayPwm, powerLevel);
        //     SetRelayPower(coolingRelayPwm, 0);
        // }
        // else if (LiquidTemperature > TargetTemperature)
        // {
        //     SetRelayPower(heaterRelayPwm, 0);
        //     SetRelayPower(coolingRelayPwm, Math.Abs(powerLevel));
        // }
        // else
        // {
        //     SetRelayPower(heaterRelayPwm, 0);
        //     SetRelayPower(coolingRelayPwm, 0);
        // }
    }
}