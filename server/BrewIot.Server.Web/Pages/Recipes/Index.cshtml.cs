using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly DeviceApiClient _deviceApiClient;

        public IndexModel(DeviceApiClient deviceApiClient)
        {
            _deviceApiClient = deviceApiClient;
        }

        public IList<Recipe> Recipe { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Recipe = await _deviceApiClient.GetRecipesAsync();
        }
    }
}
