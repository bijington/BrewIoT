using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly DeviceApiClient deviceApiClient;

        public CreateModel(DeviceApiClient deviceApiClient)
        {
            this.deviceApiClient = deviceApiClient;
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

            // string[] lines = Recipe.StepsText.Split(
            //     [Environment.NewLine],
            //     StringSplitOptions.None
            // );

            await deviceApiClient.SaveRecipeAsync(Recipe);
            // _context.Recipe.Add(Recipe);
            // await _context.SaveChangesAsync();
            //Console.WriteLine("Recipe created: " + Recipe.StepsText);

            return RedirectToPage("./Index");
        }
    }
}
