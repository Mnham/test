using WeatherStationService.Domain.Models;

namespace WeatherStationService.Domain.Contracts
{
    public interface IWeatherStation
    {
        IEnumerable<SensorData> GetSensorsData(SensorRequest request);
    }
}