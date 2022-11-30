using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Coach : User
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
