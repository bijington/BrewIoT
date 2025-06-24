using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BrewIoT.Shared.Models;

namespace BrewIoT.Device.Api
{
    public static class DeviceApiService
    {
        private static string? apiUri;

        public static void Initialize(string uri)
        {
            apiUri = uri;
        }

        public static async Task<JobStage?> GetCurrentJobStage()
        {
            using var client = new HttpClient();
            
            try
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                // TODO: add device id.
                var response = await client.GetAsync($"{apiUri}/jobstage/1");

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var values = System.Text.Json.JsonSerializer.Deserialize<JobStage>(json);
                return values;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Request timed out.");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request went sideways: {e.Message}");
                return null;
            }
        }

        public static async Task ReportReadings(int deviceId, DeviceReading reading)
        {
            using var client = new HttpClient();
            
            try
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                var json = System.Text.Json.JsonSerializer.Serialize(reading);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

                var response = await client.PostAsync($"{apiUri}/readings/{deviceId}", stringContent);

                response.EnsureSuccessStatusCode();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Request timed out.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Request went sideways: {e.Message}");
            }
        }
    }
}