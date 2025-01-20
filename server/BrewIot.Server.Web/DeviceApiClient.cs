using BrewIoT.Shared.Models;

namespace BrewIoT.Server.Web;

public class DeviceApiClient(HttpClient httpClient)
{
    public async Task<DeviceReading?> GetLatestReadingAsync(int deviceId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<DeviceReading>($"/device/latest-reading/{deviceId}",
            cancellationToken);
    }
    
    public async Task<Device[]> GetDevicesAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<Device> devices = [];

        await foreach (var device in httpClient.GetFromJsonAsAsyncEnumerable<Device>("/device", cancellationToken))
        {
            if (devices.Count >= maxItems)
            {
                break;
            }

            if (device is not null)
            {
                devices.Add(device);
            }
        }

        return devices.ToArray();
    }

    public async Task<Recipe[]> GetRecipesAsync(CancellationToken cancellationToken = default)
    {
        List<Recipe> recipes = [];

        await foreach (var recipe in httpClient.GetFromJsonAsAsyncEnumerable<Recipe>("/recipe", cancellationToken))
        {
            if (recipe is not null)
            {
                recipes.Add(recipe);
            }
        }

        return recipes.ToArray();
    }

    public async Task SaveRecipeAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("/recipe", recipe, cancellationToken);
    }
}

