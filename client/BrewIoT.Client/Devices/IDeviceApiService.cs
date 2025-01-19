using Refit;

namespace BrewIoT.Client.Devices;

public interface IDeviceApiService
{
    [Get("/device")]
    Task<Device[]> GetDevices();
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