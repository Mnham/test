using WeatherStationService.Domain.Contracts;

namespace WeatherStationService.Domain.Models
{
    public sealed class WeatherStation : IWeatherStation
    {
        private readonly List<Sensor> _sensors = new()
        {
            new Sensor(SensorPlace.Home, 600, 1200),
            new Sensor(SensorPlace.Street, 300, 600),
        };

        public IEnumerable<SensorData> GetSensorsData(SensorRequest request) => _sensors.Where(s => GetPredicate(s, request)).Select(s => s.GetData());

        private bool GetPredicate(Sensor sensor, SensorRequest request) => request switch
        {
            SensorRequest.Nothing => false,
            SensorRequest.All => true,
            SensorRequest.Home => sensor.Place == SensorPlace.Home,
            SensorRequest.Street => sensor.Place == SensorPlace.Street,
            _ => throw new NotImplementedException(),
        };
    }

    public enum SensorRequest
    {
        Nothing,
        All,
        Home,
        Street,
    }
}