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
    
    public async Task<Device?> GetDeviceAsync(int id, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Device>($"/device/{id}", cancellationToken);
    }
    
    public async Task<Device?> CreateDeviceAsync(Device device, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/device", device, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Device>(cancellationToken);
    }
    
    public async Task UpdateDeviceAsync(int id, Device device, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"/device/{id}", device, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteDeviceAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"/device/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
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

