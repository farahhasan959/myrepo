using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class PreviousJobModel
    {
        public int Id { get; set; }

        public string? Company { get; set; }
        public string? Positin { get; set; }
  
        public DateTime? Start { get; set; }
        
        public DateTime? End { get; set; }
        public bool? Freelancer { get; set; }
        public bool? StillWorking { get; set; }
    }
}
