namespace SystemMonitoring.Models
{
    public class SystemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Status { get; set; }
        public Dictionary<string, string> Metrics { get; set; }
    }
}
