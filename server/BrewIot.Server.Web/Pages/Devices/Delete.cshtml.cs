using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Devices
{
    public class DeleteModel : PageModel
    {
        private readonly DeviceApiClient _deviceApiClient;

        public DeleteModel(DeviceApiClient deviceApiClient)
        {
            _deviceApiClient = deviceApiClient;
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _deviceApiClient.GetDeviceAsync(id.Value);

            if (device is not null)
            {
                Device = device;
                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _deviceApiClient.DeleteDeviceAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
