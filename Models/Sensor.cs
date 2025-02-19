namespace OMC.Models
{
    public class Sensor
    {
        static Random random = new Random();
        public int Id { get; private set; }
        public FaceSide FaceSide { get; private set; }

        public Sensor(FaceSide faceSide)
        {
            Id = Math.Abs(Guid.NewGuid().ToString().GetHashCode());
            FaceSide = faceSide;    
        }

        public int GetTemperature()
        {
            return random.Next(50);
        }

        public SensorInfo GetInfo()
        {
            return new SensorInfo
            {
                Id = Id,
                Temperature = GetTemperature(),
                FaceSide = FaceSide
            };
        }
    }

    public class SensorInfo
    {
        public int Id { get; set; }
        public int Temperature { get; set; }
        public FaceSide FaceSide { get; set; }
    }
}
