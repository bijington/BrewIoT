using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    public static List<Device> devices = [new Device { Id = 1, Name = "Cooler box", DeviceType = DeviceType.Meadow }];
    public static ConcurrentDictionary<int, IList<DeviceReading>> deviceReadings = 
        new ConcurrentDictionary<int, IList<DeviceReading>>
        {
            [1] = 
            [
                new DeviceReading { AmbientTemperature = 19, LiquidTemperature = 15, Timestamp = DateTime.Now },
                new DeviceReading { AmbientTemperature = 19.1, LiquidTemperature = 16, Timestamp = DateTime.Now },
                new DeviceReading { AmbientTemperature = 19, LiquidTemperature = 17, Timestamp = DateTime.Now },
                new DeviceReading { AmbientTemperature = 19, LiquidTemperature = 18, Timestamp = DateTime.Now }
            ]
        };
    
    [HttpGet]
    public async Task<IActionResult> Get()
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
    
//     [HttpPost]
//     public async Task<IActionResult> AssignJob([FromBody] Device device)
//     {
//         devices.Add(device);
//         
//         return Ok(device);
//     }
//
// app.MapPost("/device/assign-job", (Job job) =>
// {
//     // TODO: check whether device already has an active job.
//     jobs.Add(job);
//
//     return Results.Created($"/job/{job.Id}", job);
// });
}
