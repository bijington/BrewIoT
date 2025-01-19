using Microsoft.EntityFrameworkCore;

namespace BrewIoT.Server.Data
{
    public class RecipeContext : DbContext
    {
        public RecipeContext (DbContextOptions<RecipeContext> options)
            : base(options)
        {
        }

        public DbSet<Web.Recipe> Recipe { get; set; } = default!;
    }
}
