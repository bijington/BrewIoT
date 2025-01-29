using System.Collections.Generic;
using System.Threading.Tasks;
using BrewIoT.Device.Meadow.Sensors;
using Meadow;

namespace BrewIoT.Device.Meadow.Controllers;

public class Pt100Controller : IController
{
    private Max31865TemperatureSensor pt100Sensor;

    public static double Pt100Temperature { get; private set; } = double.NaN;

    public Pt100Controller()
    {
        pt100Sensor = new Max31865TemperatureSensor(
            MeadowApp.Device.CreateSpiBus(),
            MeadowApp.Device.CreateDigitalOutputPort(MeadowApp.Device.Pins.D03),
            Max31865TemperatureSensor.KnownSensorType.PT100
        );

        //Resolver.SensorService.RegisterSensor(pt100Sensor);

        //pt100Sensor.StartUpdating();
    }

    public void Initialize(IReadOnlyDictionary<string, string> settings)
    {
        pt100Sensor.Initialize();
    }

    public void Write()
    {
        
    }

    async Task IController.Read()
    {
        Pt100Temperature = (await pt100Sensor.Read()).Celsius;

        Resolver.Log.Info($"ABC");
        Resolver.Log.Info($"Pt100 Temperature: {Pt100Temperature}");

        pt100Sensor.ClearFault();
        var fault = pt100Sensor.ReadFault();

        Resolver.Log.Info($"Pt100 Fault: {fault}");

        if ((fault & Max31865TemperatureSensor.Fault.HighThresh) == Max31865TemperatureSensor.Fault.HighThresh)
        {
            Resolver.Log.Info("RTD High Threshold"); 
        }
        if ((fault & Max31865TemperatureSensor.Fault.LowThresh) == Max31865TemperatureSensor.Fault.LowThresh)
        {
            Resolver.Log.Info("RTD Low Threshold"); 
        }
        if ((fault & Max31865TemperatureSensor.Fault.RefInLow) == Max31865TemperatureSensor.Fault.RefInLow)
        {
            Resolver.Log.Info("RefInLow"); 
        }
        if ((fault & Max31865TemperatureSensor.Fault.RefInHigh) == Max31865TemperatureSensor.Fault.RefInHigh)
        {
            Resolver.Log.Info("RefInHigh"); 
        }

        if ((fault & Max31865TemperatureSensor.Fault.RtdInLow) == Max31865TemperatureSensor.Fault.RtdInLow)
        {
            Resolver.Log.Info("RtdInLow"); 
        }
        if ((fault & Max31865TemperatureSensor.Fault.OvUv) == Max31865TemperatureSensor.Fault.OvUv)
        {
            Resolver.Log.Info("OvUv"); 
        }
    }
}
