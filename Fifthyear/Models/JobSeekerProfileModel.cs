using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class JobSeekerProfileModel
    {
        public ICollection<JobSeekerWorkExperience> jobSeekerWorkExperiences { get; set; } = new HashSet<JobSeekerWorkExperience>();
    }
}
