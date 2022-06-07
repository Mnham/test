using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.Extensions.Options;

using WeatherStationService.Domain.Contracts;
using WeatherStationService.Domain.Infrastructure.Configuration;
using WeatherStationService.Grpc;
using WeatherStationService.Infrastructure.Extensions;
using WeatherStationService.Infrastructure.Helpers;

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

                    MeasurementList result = new();
                    IEnumerable<Measurement> measurements = _weatherStation.GetMeasurements().Map(Mapper.SensorDataToMeasurement);
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
}