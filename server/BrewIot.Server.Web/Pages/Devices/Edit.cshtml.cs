using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Devices
{
    public class EditModel : PageModel
    {
        private readonly DeviceApiClient _deviceApiClient;

        public EditModel(DeviceApiClient deviceApiClient)
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
            
            if (device == null)
            {
                return NotFound();
            }
            
            Device = device;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _deviceApiClient.UpdateDeviceAsync(Device.Id, Device);

            return RedirectToPage("./Index");
        }
    }
}
