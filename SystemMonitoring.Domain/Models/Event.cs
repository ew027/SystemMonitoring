using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitoring.Domain.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int SystemId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string Description { get; set; }
        public bool IsResolved { get; set; }

    }
}
