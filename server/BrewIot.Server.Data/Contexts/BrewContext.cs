using Microsoft.EntityFrameworkCore;
using BrewIoT.Server.Data.Models;

namespace BrewIoT.Server.Data.Contexts;

public class BrewContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Device> Devices => Set<Device>();
    
    public DbSet<DeviceReading> DeviceReadings => Set<DeviceReading>();
    
    public DbSet<Job> Jobs => Set<Job>();
    
    public DbSet<JobStage> JobStages => Set<JobStage>();
    
    public DbSet<Recipe> Recipes => Set<Recipe>();
    
    public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure Recipe-RecipeStep relationship
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Steps)
            .WithOne()
            .HasForeignKey(s => s.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
