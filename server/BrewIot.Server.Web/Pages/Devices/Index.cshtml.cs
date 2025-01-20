using BrewIoT.Shared.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrewIoT.Server.Web.Pages.Devices
{
    public class IndexModel : PageModel
    {
        private readonly DeviceApiClient _deviceApiClient;

        public IndexModel(DeviceApiClient deviceApiClient)
        {
            _deviceApiClient = deviceApiClient;
        }

        public DeviceReading Reading { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Reading = await _deviceApiClient.GetLatestReadingAsync(1) ?? new DeviceReading();
        }
    }
}
