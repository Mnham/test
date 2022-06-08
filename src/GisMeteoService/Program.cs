using GisMeteoService.GrpcClients;

using WeatherStationService.Grpc;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddMvcCore();
builder.Services.AddHostedService<SomeHostedService>();

builder.Services.AddGrpcClient<WeatherStationApiGrpc.WeatherStationApiGrpcClient>(
    options => options.Address = new Uri("https://localhost:5001/"));

WebApplication app = builder.Build();

app.UseRouting();
app.UseEndpoints(
    b => b.MapControllers());
app.Run();