using System.ComponentModel.DataAnnotations;

namespace BrewIoT.Server.Data.Models;

public class RecipeStep
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}