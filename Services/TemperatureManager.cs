using OMC.DTOs;
using OMC.Models;

namespace OMC.Services
{
    public class TemperatureManager
    {
        const int DEFAULT_SENSOR_COUNT = 10000;
        const int MEASUREMENT_INTERVAL = 1000;

        private List<Face> Faces = [
            new Face(FaceSide.South),
            new Face(FaceSide.East),
            new Face(FaceSide.North),
            new Face(FaceSide.West),
        ];

        public TemperatureManager()
        {
            InitSensors();
            StartMeasurement();
        }

        private void InitSensors()
        {
            foreach (var face in Faces)
                for (int i = 0; i < DEFAULT_SENSOR_COUNT / Faces.Count; i++)
                    face.AddSensor();
        }

        private void StartMeasurement()
        {
            new Thread(() =>
            {
                while (true)
                {
                    AddMeasurement();
                    Thread.Sleep(MEASUREMENT_INTERVAL);
                }
            }).Start();
        }

        private void AddMeasurement()
        {
            foreach (var face in Faces)
                face.AddMeasurement();
        }

        public IEnumerable<dynamic> AggrigateLastWeek()
        {
            List<dynamic> res = new List<dynamic>();
            foreach (var face in Faces)
                res.Add(new
                {
                    face = face.Side.ToString(),
                    temperature = face.FaceHistory.GetLastWeekAggrigation()
                    //temperature = face.FaceHistory.AggrigateLastWeek()
                });
            return res;
        }

        public IEnumerable<int> GetRemovedSensors()
        {
            return Faces.SelectMany(x => x.GetRemovedSensors()).ToList();
        }

        public void AddSensor(AddSensorDto dto)
        {
            Faces.Where(x => x.Side == dto.FaceSide).First().AddSensor();
        }

        public void RemoveSensor(FaceSide side, int sensorId)
        {
            Faces.Where(x => x.Side == side).First().RemoveSensor(sensorId);
        }

        public dynamic GetMalfunctions()
        {
            return Faces.Select(x => new
            {
                face = x.Side,
                malfunction = x.GetMalfunction()
            }).ToList();
        }
    }
}
