using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BrewIoT.Server.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    // private readonly NpgsqlConnection connection;
    private readonly ILogger<JobController> logger;

    public JobController(ILogger<JobController> logger)
    {
        // this.connection = connection;
        this.logger = logger;
    }
    
    static readonly List<Job> jobs = [new Job { Id = 1, Device = new Device { Id = 3 }, Recipe = new Recipe { Id = 2, Name = "Test" } }];
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(jobs);
    }
    
    [HttpGet("stage/{deviceId}")]
    public async Task<IActionResult> GetStage(int deviceId)
    {
        return Ok(new JobStage
        {
            StartTime = DateTime.Now,
            Status = JobStageStatus.Pending,
            RecipeStep = new RecipeStep
            {
                Id = 1,
                Name = "Fermentation"
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Job job)
    {
        if (jobs.Any(x => x.Id != job.Id && x.Device.Id == job.Device.Id))
        {
            return BadRequest();
        }
        
        jobs.Add(job);
        
        return Ok(job);
    }
}
