using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class ReusmeReturnedModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string spicialization { get; set; }
        public int? JobSeekerID { get; set; }
        public List<PreviousJobModel> previousJobs { get; set; } = new List<PreviousJobModel>();
        public List<ReusmeEducationModel> reusmeEducations { get; set; } = new List<ReusmeEducationModel>();
        public List<VacancySkillModel> reusmeSkills { get; set; } = new List<VacancySkillModel>();
        public List<VacancyWorkExperienceModel> reusmeWorkExperiences { get; set; } = new List<VacancyWorkExperienceModel>();
        public List<JobSeekerLanguageModel> jobSeekerLanguages { get; set; } = new List<JobSeekerLanguageModel>();
        public string ImageFile { get; set; }
        public string Bio  { get; set; }
        public string Email { get; set; }
        public DateTime age { get; set; }

}
}
