namespace BrewIoT.Server.Data.Models;

public sealed class JobStage
{
    public int Id { get; set; }

    public Job Job { get; set; }

    public RecipeStep RecipeStep { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public JobStageStatus Status { get; set; }
}

public enum JobStageStatus
{
    Unknown,
    Pending,
    InProgress,
    Complete,
    Failed
}