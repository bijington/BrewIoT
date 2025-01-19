using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BrewIoT.Server.Data.Models;

namespace BrewIoT.Server.ApiService;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        List<Device> devices = 
        [
            new Device { Id = 1, Name = "Device 1" },
            new Device { Id = 2, Name = "Device 2" },
            new Device { Id = 3, Name = "Device 3" }
        ];
        return Ok(devices);
    }
}
