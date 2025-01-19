namespace BrewIoT.Server.Web;

public class Recipe
{
    public int Id { get; set; }

    public int Version { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime ScheduledDate { get; set; }

    public List<RecipeStep> Steps { get; set; } = [new()];

    public string StepsText { get; set; } = string.Empty;
}

public class RecipeStep
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int RecipeVersion { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Sequence { get; set; }

    public TimeSpan Duration { get; set; }

    public RecipeAction Action { get; set; }
}

public enum RecipeAction
{
    None,
    Temperature,
    Prompt
}