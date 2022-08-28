using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class JobSeekerProfileEditModel
    {
        public string Bio { get; set; }
        public string Email { get; set; }
        public ICollection<VacancyIdModel> jobSeekerWorkExperiences { get; set; } = new HashSet<VacancyIdModel>();
        public ICollection<JobSeekerEducation> jobSeekerEducations { get; set; } = new HashSet<JobSeekerEducation>();
        public ICollection<JobSeekerSkill> jobSeekerSkills { get; set; } = new HashSet<JobSeekerSkill>();
        public ICollection<JobSeekerLanguage> jobSeekerLanguages { get; set; } = new HashSet<JobSeekerLanguage>();
        public ICollection<PreviousJob> previousJobs { get; set; } = new HashSet<PreviousJob>();
    }
}
//2_ api to edit Seeker's image 
//http://farahhasan-001-site1.ctempurl.com/api/jobseekerprofiles/image/{id}
//{
//    "imageFile": "string"
//}
