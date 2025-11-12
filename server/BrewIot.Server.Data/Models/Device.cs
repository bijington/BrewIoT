using System.ComponentModel.DataAnnotations;

namespace BrewIoT.Server.Data.Models;

public sealed class Device
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public DeviceType DeviceType { get; set; }
}

public enum DeviceType
{
    Unknown,
    Meadow
}