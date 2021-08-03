using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doan.net.Models
{
    public class WorkingTime
    {
        public int WorkingTimeId { get; set; }
        public int Type { get; set; }
        public int WorkingTimeOfEmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
