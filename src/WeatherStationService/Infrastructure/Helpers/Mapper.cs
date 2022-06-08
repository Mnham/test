using WeatherStationService.Domain.Models;
using WeatherStationService.Grpc;

namespace WeatherStationService.Infrastructure.Helpers
{
    public static class Mapper
    {
        public static Measurement SensorDataToMeasurement(SensorData sensorData) => new()
        {
            Place = (int)sensorData.Place,
            DegreesCelsius = sensorData.DegreesCelsius,
            Co2Ppm = sensorData.Co2Ppm,
            Humidity = sensorData.Humidity
        };
    }
}