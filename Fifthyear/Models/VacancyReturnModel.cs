using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifthyear.Models
{
    public class VacancyReturnModel
    {
        public int Id { get; set; }
        //public int BranchId { get; set; }
        public string spicialization { get; set; }
        public string job { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public string Descritption { get; set; }
        public List<VacancyWorkExperienceModel> vacancyWorkExperiences { get; set; } = new List<VacancyWorkExperienceModel>();
        public List<VacancySkillModel> vacancySkills { get; set; } = new List<VacancySkillModel>();
        public List<VacancyLanguageModel> VacancyLanguages { get; set; } = new List<VacancyLanguageModel>();
        public List<VacancyEducationModel> vacancyEducations { get; set; } = new List<VacancyEducationModel>();
        public string? Address { get; set; }


    }
}
