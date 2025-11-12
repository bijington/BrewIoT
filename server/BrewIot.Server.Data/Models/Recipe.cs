using System.ComponentModel.DataAnnotations;

namespace BrewIoT.Server.Data.Models;

public class Recipe
{
    public int Id { get; set; }

    public int Version { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public List<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
}