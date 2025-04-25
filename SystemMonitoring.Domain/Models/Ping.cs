using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitoring.Domain.Models
{
    public class Ping
    {
        public int Id { get; set; }
        public int SystemId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int Value { get; set; }
    }
}
