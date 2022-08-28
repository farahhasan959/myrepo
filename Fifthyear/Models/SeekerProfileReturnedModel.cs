using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class SeekerProfileReturnedModel
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime age { get; set; }
        public string ImageFile { get; set; }
        public string Email { get; set; }

        public string Bio { get; set; }
        public List<JobSeekerLanguageModel> jobSeekerLanguages { get; set; } = new List<JobSeekerLanguageModel>();
        public List<ReusmeEducationModel> jobSeekerEducations { get; set; } = new List<ReusmeEducationModel>();
        public List<VacancyWorkExperienceModel> jobSeekerWorkExperiences { get; set; } = new List<VacancyWorkExperienceModel>();
        public List<VacancySkillModel> jobSeekerSkills { get; set; } = new List<VacancySkillModel>();
        public List<PreviousJobModel> previousJobs { get; set; } = new List<PreviousJobModel>();
    }
}
