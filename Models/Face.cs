namespace OMC.Models
{
    public enum FaceSide { South = 1, East = 2, North = 3, West = 4 }

    public class Face
    {
        const int MALFUNCTION_INDICATOR = 20;
        private static object _lock = new();
        public FaceSide Side { get; private set; }
        public List<Sensor> Sensors { get; private set; } = new List<Sensor>();
        public FaceHistory FaceHistory { get; private set; } = new FaceHistory();

        public Face(FaceSide side)
        {
            Side = side;
        }

        public void AddSensor()
        {
            lock (_lock)
            {
                Sensors.Add(new Sensor(Side));
            }
        }

        private void RemoveSensor(Sensor? sensor)
        {
            if (sensor == null) return;
            lock (_lock)
            {
                Sensors.Remove(sensor);
            }
        }

        public void RemoveSensor(int id)
        {
            RemoveSensor(Sensors.FirstOrDefault(x => x.Id == id));
        }

        public void AddMeasurement()
        {
            lock (_lock)
            {
                foreach (Sensor sensor in Sensors)
                    FaceHistory.AddMeasurement(DateTime.Now, sensor.GetTemperature(), sensor.Id);
            }
        }

        public IEnumerable<int> GetRemovedSensors()
        {
            return FaceHistory.GetRemovedSensors();
        }

        public int? GetMalfunction()
        {
            if (Sensors.Count < 3) return null;

            var list = new List<SensorInfo>();

            foreach (var sensor in Sensors)
                list.Add(sensor.GetInfo());

            list = list.OrderBy(x => x.Temperature).ToList();

            if (Math.Abs((list[list.Count - 1].Temperature - list[0].Temperature)) * 100 < MALFUNCTION_INDICATOR)
                return null;

            if (Math.Abs((list[1].Temperature - list[0].Temperature)) * 100 < MALFUNCTION_INDICATOR)
                return list[list.Count - 1].Id;

            return list[0].Id;
        }
    }
}
