using Grpc.Core;

using Microsoft.Extensions.Options;

using WeatherStationService.Domain.Contracts;
using WeatherStationService.Domain.Infrastructure.Configuration;
using WeatherStationService.Domain.Models;
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

        public override async Task GetMeasurements(IAsyncStreamReader<MeasurementRequest> requestStream, IServerStreamWriter<MeasurementList> responseStream, ServerCallContext context)
        {
            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    IAsyncEnumerable<MeasurementRequest> req = requestStream.ReadAllAsync();

                    SensorRequest request = SensorRequest.Nothing;
                    await foreach (MeasurementRequest item in req)
                    {
                        request = (SensorRequest)item.Request;
                    }

                    int delay = _settings.DataUpdateFrequency;
                    await Task.Delay(delay, context.CancellationToken);

                    MeasurementList result = new();
                    IEnumerable<Measurement> measurements = _weatherStation.GetSensorsData(request).Map(Mapper.SensorDataToMeasurement);
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