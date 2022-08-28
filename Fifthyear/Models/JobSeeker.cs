using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class JobSeeker:User
    {
        
        public string Gender { get; set; }
       
        public DateTime age { get; set; }
        public ICollection<JobSeekerWorkExperience> jobSeekerWorkExperiences { get; set; } = new HashSet<JobSeekerWorkExperience>();
        public ICollection<JobSeekerEducation> jobSeekerEducations { get; set; } = new HashSet<JobSeekerEducation>();
        public ICollection<JobSeekerSkill> jobSeekerSkills { get; set; } = new HashSet<JobSeekerSkill>();
        public ICollection<JobSeekerLanguage> jobSeekerLanguages { get; set; } = new HashSet<JobSeekerLanguage>();
        public ICollection<PreviousJob> previousJobs { get; set; } = new HashSet<PreviousJob>();
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public ICollection<MarkedVacancy> markedVacancies { get; set; } = new HashSet<MarkedVacancy>();
        public ICollection<Reusme> reusmes{ get; set; } = new HashSet<Reusme>();

        public virtual JobSeekerProfile JobSeekerProfile { get; set; }


    }
}
