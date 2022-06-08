using Grpc.Net.Client;

using WeatherStationService.Grpc;

namespace GisMeteoService.GrpcClients
{
    public sealed class WeatherStationClient : WeatherStationApiGrpc.WeatherStationApiGrpcClient
    {
        public WeatherStationClient(string address)
        {
            using GrpcChannel channel = GrpcChannel.ForAddress(address);
            Client = new WeatherStationApiGrpc.WeatherStationApiGrpcClient(channel);
        }

        public WeatherStationApiGrpc.WeatherStationApiGrpcClient Client { get; set; }

        public async Task Test(CancellationToken stoppingToken)
        {
            await x.RequestStream.WriteAsync(new MeasurementRequest() { Request = 1 });

            while (await x.ResponseStream.MoveNext(stoppingToken))
            {
                MeasurementList e = x.ResponseStream.Current;
            }
        }
    }

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

                foreach (var item in collection)
                {
                }

                Console.WriteLine();
            }
        }
    }
}