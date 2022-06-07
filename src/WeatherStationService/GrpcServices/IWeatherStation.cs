namespace WeatherStationService.GrpcServices
{
    public interface IWeatherStation
    {
        IEnumerable<SensorData> GetMeasurements();
    }
}