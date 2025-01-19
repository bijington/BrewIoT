using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BrewIoT.Server.Data;
using BrewIoT.Server.Web;

namespace BrewIoT.Server.Web.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly BrewIoT.Server.Data.RecipeContext _context;

        public DetailsModel(BrewIoT.Server.Data.RecipeContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.Id == id);

            if (recipe is not null)
            {
                Recipe = recipe;

                return Page();
            }

            return NotFound();
        }
    }
}
