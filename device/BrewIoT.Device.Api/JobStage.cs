namespace BrewIoT.Device.Api
{
    public sealed class JobStage
    {
        public string Name { get; set; } = string.Empty;

        public double TargetTemperature { get; set; }
    }
}