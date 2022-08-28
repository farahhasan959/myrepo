using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class ReusmeAddModel
    {
        public string Title { get; set; }
        public string spicialization { get; set; }
        public List<int> reusmeWorkExperiences { get; set; } = new List<int>();
        public List<int> reusmeEducations { get; set; } = new List<int>();
        public List<int> reusmeSkills { get; set; } = new List<int>();
    }
}
