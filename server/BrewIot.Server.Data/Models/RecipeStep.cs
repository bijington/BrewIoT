using System.ComponentModel.DataAnnotations;

namespace BrewIoT.Server.Data.Models;

public class RecipeStep
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    
    public double? TargetTemperature { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public int Order { get; set; }
    
    public int RecipeId { get; set; }
}