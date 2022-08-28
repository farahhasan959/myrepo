using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class EmployeerProfileEditModel
    {
        public string AboutUs { get; set; }
      
        public ICollection<EmployeerBranch> employeerBranches { get; set; } = new HashSet<EmployeerBranch>();
        public ICollection<EmployeerEmail> employeerEmails { get; set; } = new HashSet<EmployeerEmail>();
    }
}
