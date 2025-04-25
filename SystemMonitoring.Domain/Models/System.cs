using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMonitoring.Domain.Enums;

namespace SystemMonitoring.Domain.Models
{
    public class System
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public HostingEnvironment Environment { get; set; }
        public CheckType CheckType { get; set; }
        public string CheckUrl { get; set; }
        public int CheckInterval { get; set; } // in seconds
        public int LogRetentionPeriod { get; set; } // in days
        public int PingRetentionPeriod { get; set; } // in days
        public int WarningThreshold { get; set; } // in ms for outgoing poll, seconds for incoming ping
        public int CriticalThreshold { get; set; } // in ms for outgoing poll, seconds for incoming ping
    }
}
