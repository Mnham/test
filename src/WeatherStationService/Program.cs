using WeatherStationService.Domain.Contracts;
using WeatherStationService.Domain.Infrastructure.Configuration;
using WeatherStationService.Domain.Models;
using WeatherStationService.GrpcServices;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IWeatherStation, WeatherStation>();
builder.Services.AddMvcCore();
builder.Services.Configure<WeatherStationSettings>(builder.Configuration.GetSection(nameof(WeatherStationSettings)));

WebApplication app = builder.Build();

app.UseRouting();
app.UseEndpoints(
    b =>
    {
        b.MapControllers();
        b.MapGrpcService<WeatherStationGrpcService>();
    });
app.Run();