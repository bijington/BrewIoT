using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Devices
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
        public Device Device { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _deviceApiClient.CreateDeviceAsync(Device);

            return RedirectToPage("./Index");
        }
    }
}
