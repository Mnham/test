using WeatherStationService.Grpc;

namespace GisMeteoService.GrpcClients
{
    public class SomeHostedService : BackgroundService
    {
        private readonly IServiceProvider _provider;

        public SomeHostedService(IServiceProvider provider) => _provider = provider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using AsyncServiceScope scope = _provider.CreateAsyncScope();
            WeatherStationApiGrpc.WeatherStationApiGrpcClient client = scope.ServiceProvider.GetRequiredService<WeatherStationApiGrpc.WeatherStationApiGrpcClient>();
            Grpc.Core.AsyncDuplexStreamingCall<MeasurementRequest, MeasurementList> eventResponseStream = client.GetMeasurements(cancellationToken: stoppingToken);
            await eventResponseStream.RequestStream.WriteAsync(new MeasurementRequest() { Request = 1 }, stoppingToken);

            while (await eventResponseStream.ResponseStream.MoveNext(stoppingToken))
            {
                MeasurementList e = eventResponseStream.ResponseStream.Current;

                foreach (Measurement item in e.Measurements)
                {
                    Console.WriteLine(item.Co2Ppm);
                }
            }
        }
    }
}