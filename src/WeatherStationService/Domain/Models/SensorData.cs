namespace WeatherStationService.Domain.Models
{
    public enum SensorPlace
    {
        Home,
        Street
    }

    public struct SensorData
    {
        public int Co2Ppm { get; init; }
        public int DegreesCelsius { get; init; }
        public int Humidity { get; init; }
        public SensorPlace Place { get; init; }
        public long Timestamp { get; init; }
    }
}