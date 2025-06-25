using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;
using Npgsql;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    //private readonly NpgsqlConnection connection;
    private readonly ILogger<DeviceController> logger;

    public DeviceController(ILogger<DeviceController> logger)
    {
        //this.connection = connection;
        this.logger = logger;
    }
    
    public static List<Device> devices = [new Device { Id = 1, Name = "Cooler box", DeviceType = DeviceType.Meadow }];
    public static ConcurrentDictionary<int, IList<DeviceReading>> deviceReadings = 
        new ConcurrentDictionary<int, IList<DeviceReading>>
        {
            [1] = 
            [
                new DeviceReading { TargetTemperature = 19, LiquidTemperature = 15, Timestamp = DateTime.Now },
                new DeviceReading { TargetTemperature = 19, LiquidTemperature = 16, Timestamp = DateTime.Now.AddMinutes(1) },
                new DeviceReading { TargetTemperature = 19, LiquidTemperature = 17, Timestamp = DateTime.Now.AddMinutes(2) },
                new DeviceReading { TargetTemperature = 19, LiquidTemperature = 18, Timestamp = DateTime.Now.AddMinutes(3) }
            ]
        };
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> Get()
    {
        return Ok(devices);
    }
    
    [HttpGet("readings/{deviceId}")]
    public async Task<IActionResult> GetReadings(int deviceId)
    {
        var readings = deviceReadings[deviceId];
        return Ok(readings);
    }
    
    [HttpGet("latest-reading/{deviceId}")]
    public async Task<IActionResult> GetLatestReading(int deviceId)
    {
        var reading = deviceReadings.GetValueOrDefault(deviceId);
        
        return Ok(reading?.FirstOrDefault());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Device device)
    {
        devices.Add(device);
        
        return Ok(device);
    }
    
    [HttpPost("reading/{deviceId}")]
    public async Task<IActionResult> Post(int deviceId, [FromBody] DeviceReading deviceReading)
    {
        deviceReadings.AddOrUpdate(
            deviceId,
            [deviceReading],
            (k, v) =>
            {
                v.Add(deviceReading);
                
                return v;
            });
        
        return Ok(deviceReading);
    }
}
