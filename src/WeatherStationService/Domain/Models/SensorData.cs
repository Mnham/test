namespace WeatherStationService.Domain.Models
{
    public struct SensorData
    {
        public string Name { get; init; }
        public int DegreesCelsius { get; init; }
        public int Co2Ppm { get; init; }
        public int Humidity { get; init; }
    }
}