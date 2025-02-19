namespace OMC.Models
{
    public class FaceHistoryItem
    {
        public Sensor Sensor { get; set; } = null!;
        public int Temperature { get; set; }
        public DateTime Time { get; set; }
    }
}
