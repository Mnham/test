using Microsoft.AspNetCore.Mvc;

using WeatherStationService.Domain.Contracts;
using WeatherStationService.Domain.Models;

namespace WeatherStationService.Controllers
{
    [Route("sensors")]
    public class WeatherStationController : Controller
    {
        private readonly IWeatherStation _weatherStation;

        public WeatherStationController(IWeatherStation weatherStation) => _weatherStation = weatherStation;

        [HttpGet]
        public Task<SensorData[]> GetSensors()
        {
            SensorData[] result = _weatherStation.GetSensorsData().ToArray();

            return Task.FromResult(result);
        }
    }
}