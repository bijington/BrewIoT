using System.Collections.Concurrent;
using BrewIoT.Server.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> logger;
    private readonly BrewContext context;

    public DeviceController(ILogger<DeviceController> logger, BrewContext context)
    {
        this.logger = logger;
        this.context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> Get()
    {
        try
        {
            return Ok(await this.context.Devices.ToListAsync());
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetById(int id)
    {
        try
        {
            var device = await this.context.Devices.FindAsync(id);
            
            if (device is null)
            {
                return NotFound();
            }
            
            return Ok(new Device
            {
                Id = device.Id,
                Name = device.Name,
                DeviceType = (DeviceType)device.DeviceType
            });
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
    
    [HttpGet("readings/{deviceId}")]
    public async Task<IActionResult> GetReadings(int deviceId)
    {
        try
        {
            return Ok(await this.context.DeviceReadings.Where(d => d.DeviceId == deviceId).ToListAsync());
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return  StatusCode(500);
        }
    }
    
    [HttpGet("latest-reading/{deviceId}")]
    public async Task<IActionResult> GetLatestReading(int deviceId)
    {
        try
        {
            var latestReading = 
                await this.context.DeviceReadings
                    .OrderByDescending(d => d.Timestamp)
                    .FirstOrDefaultAsync(d => d.DeviceId == deviceId);
            
            return Ok(latestReading);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return  StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Device device)
    {
        try
        {
            var newDevice = new Data.Models.Device
            {
                Name = device.Name,
                DeviceType = (Data.Models.DeviceType)(int)device.DeviceType
            };
            
            context.Devices.Add(newDevice);
            await context.SaveChangesAsync();
            
            device.Id = newDevice.Id;
            
            return Ok(device);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return  StatusCode(500);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Device device)
    {
        try
        {
            var existingDevice = await context.Devices.FindAsync(id);
            
            if (existingDevice is null)
            {
                return NotFound();
            }
            
            existingDevice.Name = device.Name;
            existingDevice.DeviceType = (Data.Models.DeviceType)(int)device.DeviceType;
            
            await context.SaveChangesAsync();
            
            return Ok(device);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var device = await context.Devices.FindAsync(id);
            
            if (device is null)
            {
                return NotFound();
            }
            
            context.Devices.Remove(device);
            await context.SaveChangesAsync();
            
            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
    
    [HttpPost("reading/{deviceId}")]
    public async Task<IActionResult> Post(int deviceId, [FromBody] DeviceReading deviceReading)
    {
        try
        {
            var reading = new Data.Models.DeviceReading
            {
                DeviceId = deviceId,
                Timestamp = deviceReading.Timestamp,
                TargetTemperature = deviceReading.TargetTemperature,
                LiquidTemperature = deviceReading.LiquidTemperature,
                AmbientTemperature = deviceReading.AmbientTemperature
            };

            context.DeviceReadings.Add(reading);
            await context.SaveChangesAsync();
            
            return Ok(deviceReading);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return  StatusCode(500);
        }
    }
}
