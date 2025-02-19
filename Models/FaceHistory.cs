namespace OMC.Models
{
    public class FaceHistory
    {
        private Dictionary<DateTime, List<int>> TemperatureDictionary = new();
        private Dictionary<int, DateTime> SensorLastUpdates = new();

        public void AddMeasurement(DateTime date, int temperature, int sensorId)
        {
            SensorLastUpdates[sensorId] = date;

            var key = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            if (!TemperatureDictionary.ContainsKey(key))
                TemperatureDictionary.Add(key, [0, 0]);

            TemperatureDictionary[key][0]++;
            TemperatureDictionary[key][1] += temperature;
        }

        public IEnumerable<int> GetRemovedSensors()
        {
            return SensorLastUpdates.Where(x => x.Value < DateTime.Now.AddHours(-24)).Select(x => x.Key).ToList();
        }

        public dynamic GetAggrigation(DateTime from, DateTime to)
        {
            var items = TemperatureDictionary.Where(x => x.Key < to && x.Key >= from).ToList();
            return items.Select(x => new
            {
                date = x.Key,
                avg = (double)x.Value[1] / (double)x.Value[0]
            }).ToList();
        }

        public dynamic GetLastWeekAggrigation()
        {
            var date = DateTime.Now.AddDays(-7);
            var from = date.AddDays(-(int)date.DayOfWeek);
            var to = from.AddDays(7);
            return GetAggrigation(from, to);
        }
    }
}
