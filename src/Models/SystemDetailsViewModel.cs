using System.Collections.Generic;
using SystemMonitoring.Models;

namespace SystemsMonitoring.Models
{
    public class SystemDetailsViewModel : SystemViewModel
    {
        // Database server specific properties
        public List<Dictionary<string, string>> ActiveConnections { get; set; }
        public List<Dictionary<string, string>> SlowQueries { get; set; }
        public List<Dictionary<string, string>> DatabaseSizes { get; set; }

        // Web server specific properties
        public List<Dictionary<string, string>> TopUrls { get; set; }
        public List<Dictionary<string, string>> LogEntries { get; set; }
        public List<Dictionary<string, string>> WorkerProcesses { get; set; }

        public SystemDetailsViewModel()
        {
            ActiveConnections = new List<Dictionary<string, string>>();
            SlowQueries = new List<Dictionary<string, string>>();
            DatabaseSizes = new List<Dictionary<string, string>>();
            TopUrls = new List<Dictionary<string, string>>();
            LogEntries = new List<Dictionary<string, string>>();
            WorkerProcesses = new List<Dictionary<string, string>>();
        }
    }
}