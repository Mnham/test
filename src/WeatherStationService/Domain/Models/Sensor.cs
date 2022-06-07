namespace WeatherStationService.Domain.Models
{
    public sealed class Sensor
    {
        private static readonly Random _rnd = new();
        private readonly int _co2Min;
        private readonly int _co2Max;

        public Sensor(string name, int co2Min, int co2Max)
        {
            Name = name;
            _co2Min = co2Min;
            _co2Max = co2Max;

            _ = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        }

        private void Update(object _)
        {
            DegreesCelsius = _rnd.Next(20, 25);
            Co2Ppm = _rnd.Next(_co2Min, _co2Max);
            Humidity = _rnd.Next(30, 60);
        }

        public string Name { get; }
        public int DegreesCelsius { get; private set; }
        public int Co2Ppm { get; private set; }
        public int Humidity { get; private set; }

        public SensorData GetData() => new()
        {
            Name = Name,
            DegreesCelsius = DegreesCelsius,
            Co2Ppm = Co2Ppm,
            Humidity = Humidity
        };
    }
}