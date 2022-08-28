using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class Reusme
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string spicialization { get; set; }

        [ForeignKey("JobSeeker")]
        public int? JobSeekerID { get; set; }
        public JobSeeker jobSeeker{ get; set; }
        public ICollection<ReusmeWorkExperience>? reusmeWorkExperiences { get; set; } = new HashSet<ReusmeWorkExperience>();
        public ICollection<ReusmeEducation>? reusmeEducations { get; set; } = new HashSet<ReusmeEducation>();
        public ICollection<ReusmeSkill>? reusmeSkills { get; set; } = new HashSet<ReusmeSkill>();
        public ICollection<NotificationResponse>? notifications  {get; set; } = new HashSet<NotificationResponse>();
        public Reusme Clone()
        {
            return this.MemberwiseClone() as Reusme;
        }
    }
}
