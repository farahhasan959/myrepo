using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class ReusmetoseekerModel
    {
        public int Id { get; set; }
        public string spicialization { get; set; }
        public string Title { get; set; }
        public List<ReusmeEducationModel> reusmeEducations { get; set; } = new List<ReusmeEducationModel>();
        public List<VacancySkillModel> reusmeSkills { get; set; } = new List<VacancySkillModel>();
        public List<VacancyWorkExperienceModel> reusmeWorkExperiences { get; set; } = new List<VacancyWorkExperienceModel>();
    }
}
