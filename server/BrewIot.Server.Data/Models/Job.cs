namespace BrewIoT.Server.Data.Models;

public sealed class Job
{
    public int Id { get; set; }

    public int DeviceId { get; set; }
    
    public Device Device { get; set; }

    public Recipe Recipe { get; set; }
}