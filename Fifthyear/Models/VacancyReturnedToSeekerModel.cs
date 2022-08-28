using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class VacancyReturnedToSeekerModel
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        
        public string Name { get; set; }
        public string ImageFile { get; set; }
        public string spicialization { get; set; }
        public string job { get; set; }
        public string? Address { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public string Descritption { get; set; }
        public bool marked { get; set; }
        public List<VacancyWorkExperienceModel> vacancyWorkExperiences { get; set; } = new List<VacancyWorkExperienceModel>();
        public List<VacancySkillModel> vacancySkills { get; set; } = new List<VacancySkillModel>();
        public List<VacancyLanguageModel> VacancyLanguages { get; set; } = new List<VacancyLanguageModel>();
        public List<VacancyEducationModel> vacancyEducations { get; set; } = new List<VacancyEducationModel>();
        

    }
}
