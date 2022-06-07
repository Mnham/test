using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.Extensions.Options;

using WeatherStationService.Grpc;

namespace WeatherStationService.GrpcServices
{
    public class WeatherStationGrpcService : WeatherStationApiGrpc.WeatherStationApiGrpcBase
    {
        private readonly IWeatherStation _weatherStation;
        private readonly WeatherStationSettings _settings;

        public WeatherStationGrpcService(IWeatherStation weatherStation, IOptions<WeatherStationSettings> options)
        {
            _weatherStation = weatherStation;
            _settings = options.Value;
        }

        public override async Task GetMeasurements(Empty request, IServerStreamWriter<MeasurementList> responseStream, ServerCallContext context)
        {
            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    int delay = _settings.DataUpdateFrequency;
                    await Task.Delay(delay, context.CancellationToken);

                    IEnumerable<Measurement> measurements = _weatherStation.GetMeasurements().Map(DtoMapper.SensorDataToMeasurement);
                    MeasurementList result = new();
                    foreach (Measurement measurement in measurements)
                    {
                        result.Measurements.Add(measurement);
                    }

                    await responseStream.WriteAsync(result, context.CancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    public sealed class WeatherStationSettings
    {
        public int DataUpdateFrequency { get; set; }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<Out> Map<In, Out>(this IEnumerable<In> source, Func<In, Out> mapper)
        {
            foreach (In item in source)
            {
                yield return mapper(item);
            }
        }
    }

    public static class DtoMapper
    {
        public static Measurement SensorDataToMeasurement(SensorData sensorData) => new()
        {
            Sensor = sensorData.Name,
            DegreesCelsius = sensorData.DegreesCelsius,
            Co2Ppm = sensorData.Co2Ppm,
            Humidity = sensorData.Humidity
        };
    }

    public sealed class WeatherStation : IWeatherStation
    {
        private List<Sensor> Sensor { get; } = new()
        {
            new Sensor("Home"),
            new Sensor("Street"),
        };

        public IEnumerable<SensorData> GetMeasurements() => Sensor.Select(s => s.GetData());
    }

    public sealed class Sensor
    {
        private static readonly Random _rnd = new();
        private readonly Timer _timer;

        public Sensor(string name)
        {
            Name = name;
            _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            DegreesCelsius = _rnd.Next(20, 25);
            Co2Ppm = _rnd.Next(1000, 2000);
            Humidity = _rnd.Next(30, 60);
        }

        private DateTime dateTime;

        private void Update(object _)
        {
            //todo рандом
            dateTime = DateTime.Now;
            DegreesCelsius = _rnd.Next(20, 25);
            Co2Ppm = 1200;
            Humidity = 40;
        }

        public string Name { get; }
        public int DegreesCelsius { get; private set; }
        public int Co2Ppm { get; private set; }
        public int Humidity { get; private set; }

        public SensorData GetData() => new()
        {
            Name = Name,
            DegreesCelsius = 20,
            Co2Ppm = 1200,
            Humidity = 40
        };
    }

    public struct SensorData
    {
        public string Name { get; init; }
        public int DegreesCelsius { get; init; }
        public int Co2Ppm { get; init; }
        public int Humidity { get; init; }
    }
}