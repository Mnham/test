using WeatherStationService.Domain.Contracts;

namespace WeatherStationService.Domain.Models
{
    public sealed class WeatherStation : IWeatherStation
    {
        private readonly List<Sensor> _sensors = new()
        {
            new Sensor("Home", 600, 1200),
            new Sensor("Street", 300, 600),
        };

        public IEnumerable<SensorData> GetMeasurements() => _sensors.Select(s => s.GetData());
    }
}