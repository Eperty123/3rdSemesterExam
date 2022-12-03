using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AvailableTimesDTO
    {
        public int CoachId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
