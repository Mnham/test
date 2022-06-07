using Microsoft.AspNetCore.Mvc;

using WeatherStationService.GrpcServices;

namespace WeatherStationService.Controllers
{
    [Route("events")]
    public class WeatherStationController : Controller
    {
        private readonly IWeatherStation _weatherStation;

        public WeatherStationController(IWeatherStation weatherStation) => _weatherStation = weatherStation;

        [HttpGet]
        public Task<List<SensorData>> GetEvent()
        {
            List<SensorData> result = _weatherStation.GetMeasurements().ToList();

            return Task.FromResult(result);
        }
    }
}