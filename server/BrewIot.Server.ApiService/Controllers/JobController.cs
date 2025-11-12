using BrewIoT.Server.Data.Contexts;
using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly ILogger<JobController> logger;
    private readonly BrewContext context;

    public JobController(ILogger<JobController> logger, BrewContext context)
    {
        this.logger = logger;
        this.context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await this.context.Jobs.ToListAsync());
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
    
    [HttpGet("stage/{deviceId}")]
    public async Task<IActionResult> GetStage(int deviceId)
    {
        try
        {
            var currentStage = 
                await this.context.JobStages
                    .FirstOrDefaultAsync(stage => stage.Job.DeviceId == deviceId && stage.Status == Data.Models.JobStageStatus.InProgress);
            
            return Ok(currentStage);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Job job)
    {
        try
        {
            var existingJob = await this.context.Jobs
                .FirstOrDefaultAsync(x => x.DeviceId == job.Device.Id);
            
            if (existingJob != null)
            {
                return BadRequest();
            }
            
            var newJob = new Data.Models.Job
            {
                DeviceId = job.Device.Id
            };
            
            context.Jobs.Add(newJob);
            await context.SaveChangesAsync();
            
            return Ok(job);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            
            return StatusCode(500);
        }
    }
}
