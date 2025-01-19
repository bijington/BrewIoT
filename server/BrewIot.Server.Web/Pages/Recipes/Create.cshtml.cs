using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BrewIoT.Server.Data;
using BrewIoT.Server.Web;

namespace BrewIoT.Server.Web.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly DeviceApiClient _deviceApiClient;

        public CreateModel(DeviceApiClient deviceApiClient)
        {
            _deviceApiClient = deviceApiClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = new();

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string[] lines = Recipe.StepsText.Split(
                [Environment.NewLine],
                StringSplitOptions.None
            );

            await _deviceApiClient.SaveRecipeAsync(Recipe);
            // _context.Recipe.Add(Recipe);
            // await _context.SaveChangesAsync();
            Console.WriteLine("Recipe created: " + Recipe.StepsText);

            return RedirectToPage("./Index");
        }
    }
}
