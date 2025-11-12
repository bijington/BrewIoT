namespace BrewIoT.Server.Data.Models;

public class DeviceReading
{
    public double LiquidTemperature { get; set; }

    public double AmbientTemperature { get; set; }

    public double TargetTemperature { get; set; }

    public DateTime Timestamp { get; set; }
    
    public int DeviceId { get; set; }
    
    public Device Device { get; set; }
}