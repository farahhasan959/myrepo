using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class Indice
    { 
        [Key]
        public int ID { get; set; }
        public int Module { get; set; }
        public string Value { get; set; }

        //public int Order { get; set; }
        /*
        public ICollection<ReusmeEducation> reusmeEducations { get; set; } = new HashSet<ReusmeEducation>();
        public ICollection<ReusmeSkill> reusmeSkills { get; set; } = new HashSet<ReusmeSkill>();
        public ICollection<JobSeekerEducation> jobSeekerEducations { get; set; } = new HashSet<JobSeekerEducation>();
        public ICollection<JobSeekerSkill> jobSeekerSkills { get; set; } = new HashSet<JobSeekerSkill>();
        */
    }
}
