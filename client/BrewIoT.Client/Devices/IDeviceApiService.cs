using BrewIoT.Shared.Models;
using Refit;

namespace BrewIoT.Client.Devices;

public interface IDeviceApiService
{
    [Get("/device")]
    Task<Device[]> GetDevices();
    
    [Get("/device/readings/{deviceId}")]
    Task<DeviceReading[]> GetDeviceReadings(int deviceId);
}

public sealed class Device
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public DeviceType DeviceType { get; set; }
}

public enum DeviceType
{
    Unknown,
    Meadow
}