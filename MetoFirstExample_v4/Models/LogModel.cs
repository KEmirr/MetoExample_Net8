namespace MetoFirstExample_v4_WepAPI.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Source { get; set; }
    }
}
