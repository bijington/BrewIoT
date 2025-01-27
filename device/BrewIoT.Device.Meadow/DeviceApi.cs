using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BrewIoT.Device.Meadow;

public static class DeviceApiService
{
    static DeviceApiService() { }

    private static string apiUri;

    public static void Initialize(string uri)
    {
        apiUri = uri;
    }

    // public static async Task<JobStage> GetCurrentJobStage()
    // {
    //     using (HttpClient client = new HttpClient())
    //     {
    //         try
    //         {
    //             client.Timeout = new TimeSpan(0, 5, 0);

    //             // TODO: add device id.
    //             HttpResponseMessage response = await client.GetAsync($"{apiUri}/jobstage/1");

    //             response.EnsureSuccessStatusCode();
    //             string json = await response.Content.ReadAsStringAsync();
    //             var values = System.Text.Json.JsonSerializer.Deserialize<JobStage>(json);
    //             return values;
    //         }
    //         catch (TaskCanceledException)
    //         {
    //             Console.WriteLine("Request timed out.");
    //             return null;
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine($"Request went sideways: {e.Message}");
    //             return null;
    //         }
    //     }
    // }

    // public static async Task ReportReadings(int deviceId, Readings readings)
    // {
    //     using (HttpClient client = new HttpClient())
    //     {
    //         try
    //         {
    //             client.Timeout = new TimeSpan(0, 5, 0);

    //             var json = System.Text.Json.JsonSerializer.Serialize(readings);
    //             var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

    //             HttpResponseMessage response = await client.PostAsync($"{apiUri}/readings/{deviceId}", stringContent);

    //             response.EnsureSuccessStatusCode();
    //         }
    //         catch (TaskCanceledException)
    //         {
    //             Console.WriteLine("Request timed out.");
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine($"Request went sideways: {e.Message}");
    //         }
    //     }
    // }
}

public sealed class JobStage
{
    public string Name { get; set; }
    public double TargetTemperature { get; set; }
}

public enum HeatingMode
{
    Off,
    Heating,
    Cooling
}

public class Readings
{
    public double LiquidTemperature { get; set; }

    public double AmbientTemperature { get; set; }

    public double InternalAirTemperature { get; set; }

    public HeatingMode HeatingMode { get; set; }

    public HeatingMode Action { get; set; }
}