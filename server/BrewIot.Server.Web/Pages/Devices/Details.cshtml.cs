using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Devices
{
    public class DetailsModel : PageModel
    {
        private readonly DeviceApiClient _deviceApiClient;

        public DetailsModel(DeviceApiClient deviceApiClient)
        {
            _deviceApiClient = deviceApiClient;
        }

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
    }
}
