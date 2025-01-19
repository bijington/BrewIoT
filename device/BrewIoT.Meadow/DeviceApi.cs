using System;
// using System.Net.Http;
// using System.Threading.Tasks;

// namespace BrewIot.Meadow;

// public static class DeviceApiService
// {
//     static string apiUri = "https://localhost:7466/device";

//     static DeviceApiService() { }

//     public static async Task<JobStage> GetCurrentJobStage()
//     {
//         using (HttpClient client = new HttpClient())
//         {
//             try
//             {
//                 client.Timeout = new TimeSpan(0, 5, 0);

//                 HttpResponseMessage response = await client.GetAsync($"{apiUri}/jobstage");

//                 response.EnsureSuccessStatusCode();
//                 string json = await response.Content.ReadAsStringAsync();
//                 var values = System.Text.Json.JsonSerializer.Deserialize<JobStage>(json);
//                 return values;
//             }
//             catch (TaskCanceledException)
//             {
//                 Console.WriteLine("Request timed out.");
//                 return null;
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine($"Request went sideways: {e.Message}");
//                 return null;
//             }
//         }
//     }

//     public static async Task ReportReadings(int deviceId, Readings readings)
//     {
//         using (HttpClient client = new HttpClient())
//         {
//             try
//             {
//                 client.Timeout = new TimeSpan(0, 5, 0);

//                 var json = System.Text.Json.JsonSerializer.Serialize(readings);
//                 var stringContent = new StringContent(json, System.Text.UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

//                 HttpResponseMessage response = await client.PostAsync($"{apiUri}/readings/{deviceId}", stringContent);

//                 response.EnsureSuccessStatusCode();
//             }
//             catch (TaskCanceledException)
//             {
//                 Console.WriteLine("Request timed out.");
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine($"Request went sideways: {e.Message}");
//             }
//         }
//     }
// }

public sealed class JobStage
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public int RecipeStepId { get; set; }

    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public TimeSpan Duration { get; set; }

    public DateTime EndTime { get; set; }

    public JobStageStatus Status { get; set; }

    public int TargetTemperature { get; set; }

    public HeatingMode HeatingMode { get; set; }
}

public enum HeatingMode
{
    Off,
    Heating,
    Cooling
}

public enum JobStageStatus
{
    Unknown,
    Pending,
    InProgress,
    Complete,
    Failed
}

public class Readings
{
    public double LiquidTemperature { get; set; }

    public double AmbientTemperature { get; set; }

    public double InternalAirTemperature { get; set; }

    public HeatingMode HeatingMode { get; set; }

    public HeatingMode Action { get; set; }
}